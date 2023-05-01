using System.Drawing.Drawing2D;

namespace WinFormsApp1
{
    public class MathHelper
    {
        public List<List<double>> transposeMatrix(List<List<double>> matrix)
        {
            List<List<double>> result = new List<List<double>>();

            for (int i = 0 ; i < matrix[0].Count; i++)
            {
                result.Add(new List<double>());
            }

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[0].Count; j++)
                {
                    result[j].Add(matrix[i][j]);
                }
            }

            return result;
        }

        public List<List<double>> backMatrix(List<List<double>> matrix)
        {
            List<List<double>> result = new();

            const double tiny = 0.00001;

            // Build the augmented matrix.
            int num_rows = matrix.Count;
            double[,] augmented = new double[num_rows, 2 * num_rows];
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                    augmented[row, col] = matrix[row][col];
                augmented[row, row + num_rows] = 1;
            }

            // num_cols is the number of the augmented matrix.
            int num_cols = 2 * num_rows;

            // Solve.
            for (int row = 0; row < num_rows; row++)
            {
                // Zero out all entries in column r after this row.
                // See if this row has a non-zero entry in column r.
                if (Math.Abs(augmented[row, row]) < tiny)
                {
                    // Too close to zero. Try to swap with a later row.
                    for (int r2 = row + 1; r2 < num_rows; r2++)
                    {
                        if (Math.Abs(augmented[r2, row]) > tiny)
                        {
                            // This row will work. Swap them.
                            for (int c = 0; c < num_cols; c++)
                            {
                                double tmp = augmented[row, c];
                                augmented[row, c] = augmented[r2, c];
                                augmented[r2, c] = tmp;
                            }
                            break;
                        }
                    }
                }

                // If this row has a non-zero entry in column r, use it.
                if (Math.Abs(augmented[row, row]) > tiny)
                {
                    // Divide the row by augmented[row, row] to make this entry 1.
                    for (int col = 0; col < num_cols; col++)
                        if (col != row)
                            augmented[row, col] /= augmented[row, row];
                    augmented[row, row] = 1;

                    // Subtract this row from the other rows.
                    for (int row2 = 0; row2 < num_rows; row2++)
                    {
                        if (row2 != row)
                        {
                            double factor = augmented[row2, row] / augmented[row, row];
                            for (int col = 0; col < num_cols; col++)
                                augmented[row2, col] -= factor * augmented[row, col];
                        }
                    }
                }
            }

            // See if we have a solution.
            if (augmented[num_rows - 1, num_rows - 1] == 0) return null;

            // Extract the inverse array.
            double[,] inverse = new double[num_rows, num_rows];
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                {
                    inverse[row, col] = augmented[row, col + num_rows];
                }
            }

            
            for (int i = 0; i < num_rows; i++)
            {
                result.Add(new List<double>());
                for (int j = 0; j < num_rows; j++)
                {
                    result[i].Add(inverse[i, j]);
                }
            }

            return result;
        }

        public List<List<double>> multiplyMatrixes(List<List<double>> matrix1, List<List<double>> matrix2) 
        {
            List<List<double>> result = new List<List<double>>();
            for (int i = 0; i < matrix1[0].Count; i++)
            {
                result.Add(new List<double>());
            }

            List<List<double>> tempInversed = new();

            for (int i = 0; i < matrix1[0].Count; i++)
            {
                List<double> temp = new List<double>();
                for (int j = 0; j < matrix1.Count; j++)
                {
                    temp.Add(matrix1[j][i]);
                }
                tempInversed.Add(temp);
            }

            for (int i = 0; i < matrix1[0].Count; i++)
            {
                for (int j = 0; j < matrix1[0].Count; j++)
                {
                    result[i].Add(this.multiplyLists(tempInversed[i], matrix2[j]));
                }
            }

            return result;
        }

        private double multiplyLists(List<double> l1, List<double> l2)
        {
            double res = 0;
            for (int i = 0; i < l1.Count; i++)
            {
                res += l1[i] * l2[i];
            }
            return res;
        }

        public List<double> multiplyMatrix(List<List<double>> matrix, List<double> y) 
        { 
            List<double> result = new List<double>();

            for (int i = 0; i < matrix[0].Count; i++)
            {
                double tmp = 0;
                for (int j = 0; j < matrix.Count; j++)
                {
                    tmp += matrix[j][i] * y[j];
                }
                result.Add(tmp);
            }

            return result;
        }

        public List<List<double>> multiplyMatrixToNumber(List<List<double>> matrix, double y)
        {
            for (int i = 0; i < matrix[0].Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    matrix[i][j] *= y;
                }
            }

            return matrix;
        }

        public double getSumListsMultiplied(List<double> x, List<double> y)
        {
            double sum = 0;

            for (int i = 0; i < x.Count; i++)
            {
                sum += x[i] * y[i];
            }

            return sum;
        }

        public double getAverageValue(List<double> values)
        {
            double sum = 0;

            foreach (double v in values)
            {
                sum += v;
            }

            return sum / values.Count;
        }

        public double getAverageValueSquared(List<double> values)
        {
            double sum = 0;

            foreach (double v in values)
            {
                sum += Math.Pow(v, 2);
            }

            return sum / values.Count;
        }

        public double getSumValue(List<double> values)
        {
            double sum = 0;

            foreach (double v in values)
            {
                sum += v;
            }

            return sum;
        }

        public List<double> multiplyVectorToValue(List<double> values, double multiplier)
        {
            List<double> sum = new List<double>();

            foreach (double v in values)
            {
                sum.Add(v * multiplier);
            }

            return sum;
        }

        public double getSumSquared(List<double> values)
        {
            double sum = 0;

            foreach (double v in values)
            {
                sum += Math.Pow(v, 2);
            }

            return sum;
        }

        public double getSquaredDifference(List<double> x, List<double> y, double a0, double a1)
        {
            double sum = 0;

            for (int i = 0; i < x.Count; i++)
            {
                sum += Math.Pow(y[i] - (a0 + a1 * x[i]) , 2);
            }
            
            return sum;
        }
    }
}
