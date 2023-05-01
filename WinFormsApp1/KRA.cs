using System.Diagnostics;

namespace WinFormsApp1
{
    public class KRAResultItem
    {
        public Double r { get; set; }
    };

    public class KRA
    {
        private MathHelper _mathHelper = new MathHelper();
        public List<KRAResultItem> _result = new();
        List<List<double>> _values;
        List<List<double>> valuesTransposed = new();
        List<double> transposedYMultiply = new();
        List<List<double>> transposedMultiply = new();
        List<List<double>> transposedMultiplyBack = new();
        List<List<double>> initialValuesTransposed = new();
        public List<double> finalKoefs = new(); // коэфициенты уравнения регрессии
        public KraResult result = new(); //Средняя ошибка аппроксимации
        List<double> _y;
        public List<List<double>> pairCorrelations = new();

        public void setValues(List<List<double>> values, List<double> y) {
            this._values = values;
            this._y = y;
        }

        public void startKra()
        {
            for (int i = 0; i < this._values.Count; i++)
            {
                List<double> list = new();
                for (int j = 0; j < this._values.Count; j++)
                {
                    list.Add(this.calculatePairCorrelations(this._values[i], this._values[j]));
                }
                list.Add(this.calculatePairCorrelations(this._values[i], this._y));
                this.pairCorrelations.Add(list);
            }
            List<double> listY = new();
            for (int i = 0; i < this._values.Count; i++)
            {
                listY.Add(this.calculatePairCorrelations(this._y, this._values[i]));
            }
            listY.Add(this.calculatePairCorrelations(this._y, this._y));
            this.pairCorrelations.Add(listY); // парные кореляции

            for (int i = 0; i < this._values.Count; i++)
            {
                KRAResultItem item = new();
                item.r = this.calculateR(this._values[i], this._y);
                _result.Add(item);
            }

            List<double> ones = new List<double>();
            for (int i = 0; i < _y.Count; i++)
            {
                ones.Add(1);
            }

            this.initialValuesTransposed = this._mathHelper.transposeMatrix(this._values);
            this._values.Insert(0, ones);
            this.valuesTransposed = this._mathHelper.transposeMatrix(this._values);
          
            this.transposedMultiply = this._mathHelper.multiplyMatrixes(this.valuesTransposed, this._values);
            this.transposedYMultiply = this._mathHelper.multiplyMatrix(this.valuesTransposed, this._y);
            this.transposedMultiplyBack = this._mathHelper.backMatrix(this.transposedMultiply);
            this.finalKoefs = this._mathHelper.multiplyMatrix(this.transposedMultiplyBack, this.transposedYMultiply);
            this.result = this.calcKraResults();
        }

        private KraResult calcKraResults()
        {
            double epsilonSquared = 0; // оценка дисперсии
            double epsilonY = 0; 

            for (int i = 0; i < this._y.Count; i++)
            {
                double predict = this.calcYPredict(this.initialValuesTransposed[i]);
                double epsilon = this._y[i] - predict;
                epsilonSquared += Math.Pow(epsilon, 2);
                epsilonY += Math.Abs(epsilon / this._y[i]);
            }

            double avgApproximation = epsilonY / this._y.Count * 100; // средняя ошибка аппроксимации
            double unshiftedDispersion = (1 / (this._y.Count - (this._values.Count - 1) - 1)) * epsilonSquared; // несмещенная оценка дисперсии 
            double avgSquaredShift = Math.Sqrt(unshiftedDispersion); // среднеквадратическое отклонение

            List<double> paramsDispersion = new(); // дисперсия параметров
            List<List<double>> covariantMatrix = this._mathHelper.multiplyMatrixToNumber(this.transposedMultiplyBack, unshiftedDispersion); // оценка ковариационной матрицы вектора

            for (int i = 0; i < covariantMatrix.Count; i++)
            {
                paramsDispersion.Add(Math.Sqrt(covariantMatrix[i][i]));
            }

            List<double> elastics = new(); // частные коэфициенты эластичности

            for (int i = 1; i < this._values.Count; i++)
            {
                elastics.Add(this.finalKoefs[i] * this._mathHelper.getAverageValue(this._values[i]) / this._mathHelper.getAverageValue(this._y));
            }

            List<double> studentCriterias = new(); // t - критерии стьюдента

            for (int i = 0; i < this.finalKoefs.Count; i++)
            {
                studentCriterias.Add(Math.Abs(finalKoefs[i] / paramsDispersion[i]));
            }
            double lower = this.calcRLower();
            double rSquared = Math.Round(1 - (epsilonSquared / (lower)), 4);

            double fisherUpper = rSquared / (1 - rSquared);

            double fisherLowerUpper = this._y.Count - (this._values.Count - 1) - 1;
            double fisherLowerLower = this._values.Count - 1;

            double fisherLower = fisherLowerUpper / fisherLowerLower;

            double fisher = fisherUpper * fisherLower;
            KraResult res = new();

            res.fisher = fisher;
            res.epsilonY = epsilonY;
            res.epsilonSquared = epsilonSquared;
            res.rSquared = rSquared;
            res.avgSquaredShift = avgSquaredShift;
            res.avgApproximation = avgApproximation;
            res.elastics = elastics;
            res.studentCriterias = studentCriterias;
            res.unshiftedDispersion = unshiftedDispersion;
            res.paramsDispersion = paramsDispersion;

            return res;
        }

        private double calcRLower()
        {
            double res = 0;
            double avgY = this._mathHelper.getAverageValue(this._y);

            for (int i = 0; i < this._y.Count; i++)
            {
                res += Math.Pow(this._y[i] - avgY, 2);
            }

            return res;
        }

        public double calcYPredict(List<double> x) //расчитать значение по уравнению регрессии
        {
            double res = this.finalKoefs[0];

            for (int i = 0; i < x.Count; i++)
            {
                res += x[i] * this.finalKoefs[i + 1];
            }

            return res;
        }

        private double calculateR(List<double> x, List<double> y) => (this._mathHelper.getSumListsMultiplied(x, y) - (this._mathHelper.getSumValue(x) * this._mathHelper.getSumValue(y) / x.Count)) / Math.Sqrt((this._mathHelper.getSumSquared(x) - (Math.Pow(this._mathHelper.getSumValue(x), 2)) / x.Count) * (this._mathHelper.getSumSquared(y) - (Math.Pow(this._mathHelper.getSumValue(y), 2)) / x.Count));
    
        private double calculatePairCorrelations(List<double> x, List<double> y)
        {    
            double upper = 0;
            for (int i = 0; i < x.Count; i++)
            {
                upper += (x[i] - this._mathHelper.getAverageValue(x)) * (y[i] - this._mathHelper.getAverageValue(y));
            }

            double lowerFirst = 0;
            for (int i = 0; i < x.Count; i++)
            {
                lowerFirst += Math.Pow(x[i] - this._mathHelper.getAverageValue(x), 2);
            }
            double lowerSecond = 0;
            for (int i = 0; i < x.Count; i++)
            {
                lowerSecond += Math.Pow(y[i] - this._mathHelper.getAverageValue(y), 2);
            }
            double lower = Math.Sqrt(lowerFirst * lowerSecond);

            return upper / lower;
        }
    }
}
