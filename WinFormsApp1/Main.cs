using System;

namespace WinFormsApp1
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            labelDebug.Text = UserStore.role;
            switch(UserStore.role)
            {
                case "operator":
                    tabControl1.TabPages[2].Parent = null;
                    break;
                case "rukovoditel":
                    // Все вкладки (кроме введения?)
                    break;
                case "admin":
                    // Все вкладки
                    break;
            }
            this.toggleLabels(false);
            dataGridView3.Rows.Add(-12, 2500, 2450, 50000, 5, 200);
            dataGridView3.Rows.Add(-13, 2500, 3500, 0, 0, 125);
            dataGridView3.Rows.Add(-5, 3550, 2525, 32000, 10, 150);
            dataGridView3.Rows.Add(7, 2500, 2500, 41000, 10, 220);
            dataGridView3.Rows.Add(16, 3550, 4600, 5000, 15, 260);
            dataGridView3.Rows.Add(20, 2450, 2500, 22000, 0, 230);
            dataGridView3.Rows.Add(22, 2500, 2550, 40000, 5, 280);

            dataGridView4.Rows.Add(-12, 2500, 2450, 50000, 5, 200);
            dataGridView4.Rows.Add(-13, 2500, 3500, 0, 0, 125);
            dataGridView4.Rows.Add(-5, 3550, 2525, 32000, 10, 150);
            dataGridView4.Rows.Add(7, 2500, 2500, 41000, 10, 220);
            dataGridView4.Rows.Add(16, 3550, 4600, 5000, 15, 260);
            dataGridView4.Rows.Add(20, 2450, 2500, 22000, 0, 230);
            dataGridView4.Rows.Add(22, 2500, 2550, 40000, 5, 280);
        }

        protected KRA kraAl = new KRA();
        protected KRA kraCu = new KRA();

        private void toggleLabels(bool value)
        {
            label3.Visible = value;
            label5.Visible = value;
            label9.Visible = value;
        }

        private List<double> readDataGrid1(int index)
        {
            List<double> data = new List<double>();

            for (int rows = 0; rows < dataGridView3.Rows.Count - 1; rows++)
            {
                try
                {
                    data.Add(Convert.ToDouble(dataGridView3.Rows[rows].Cells[index].Value));
                }
                //catch (Exception exc)
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                    //MessageBox.Show("Некоректный ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return data;
        }

        private List<double> readDataGrid2(int index)
        {
            List<double> data = new List<double>();

            for (int rows = 0; rows < dataGridView4.Rows.Count - 1; rows++)
            {
                try
                {
                    data.Add(Convert.ToDouble(dataGridView4.Rows[rows].Cells[index].Value));
                }
                //catch (Exception exc)
                catch (Exception exc)
                {
                    throw new Exception(exc.Message);
                    //MessageBox.Show("Некоректный ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return data;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            double max = 0;
            string maxi = "";

            foreach (DataGridViewRow row in this.dataGridView2.Rows)
            {

                if (Convert.ToString(row.Cells[0].Value) == "")
                    break;
                max = Convert.ToDouble(row.Cells[1].Value);
                for (int i = 2; i < 5; i++)
                {
                    if (max < Convert.ToDouble(row.Cells[i].Value))
                    {
                        max = Convert.ToDouble(row.Cells[i].Value);
                    }
                }

                row.Cells[5].Value = max.ToString();
            }
            foreach (DataGridViewRow row in this.dataGridView2.Rows)
            {
                if (max < Convert.ToDouble(row.Cells[5].Value))
                {
                    max = Convert.ToDouble(row.Cells[5].Value);
                    maxi = row.Cells[0].Value.ToString()!;
                    row.Selected = true;
                }
            }
            label12.Text = maxi;
            label12.Visible = true;
            label11.Visible = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void calcKraAl()
        {
            List<List<double>> aluminium = new List<List<double>>();
            List<double> aluminiumResult = readDataGrid1(5);
            resultLabel.Text = "Степень кореляции";
            resultFuncLabel.Text = "Результатирующая функция";
            fisherLabel.Text = "Критерий Фишера";

            aluminium.Add(readDataGrid1(0));
            aluminium.Add(readDataGrid1(1));
            aluminium.Add(readDataGrid1(2));
            aluminium.Add(readDataGrid1(3));
            aluminium.Add(readDataGrid1(4));

            kraAl.setValues(
                aluminium, aluminiumResult
            );
            kraAl.startKra();
            resultLabel.Text += "\n";

            for (int i = 0; i < kraAl._result.Count; i++)
            {
                if (kraAl._result[i].r >= 0.7)
                {
                    dataGridView3.Columns[i].HeaderCell.Style.BackColor = Color.Green;
                }
                if (kraAl._result[i].r < 0.7 && kraAl._result[i].r >= 0.3)
                {
                    dataGridView3.Columns[i].HeaderCell.Style.BackColor = Color.Yellow;
                }
                if (kraAl._result[i].r < 0.3)
                {
                    dataGridView3.Columns[i].HeaderCell.Style.BackColor = Color.Red;
                }
            }

            for (int j = 0; j < kraAl.finalKoefs.Count; j++)
            {
                if (j == 0)
                {
                    resultFuncLabel.Text += " " + kraAl.finalKoefs[0];
                    continue;
                }
                resultFuncLabel.Text += $" + {kraAl.finalKoefs[j]} x{j}";
            }

            fisherLabel.Text += $": {kraAl.result.fisher}";

            for (int i = 0; i < kraAl.pairCorrelations.Count; i++)
            {
                pairAlGrid.Rows.Add(
                    kraAl.pairCorrelations[i][0],
                    kraAl.pairCorrelations[i][1],
                    kraAl.pairCorrelations[i][2],
                    kraAl.pairCorrelations[i][3],
                    kraAl.pairCorrelations[i][4],
                    kraAl.pairCorrelations[i][5]
                );
                if (i != 5)
                {
                    pairAlGrid.Rows[i].HeaderCell.Value = $"x{i + 1}";
                }
                else
                {
                    pairAlGrid.Rows[i].HeaderCell.Value = "y";
                }
            }

            for (int i = 0; i < kraAl.result.elastics.Count; i++)
            {
                label2.Text += $"\nE{i + 1} = {kraAl.result.elastics[i]}";
            }

            for (int i = 0; i < kraAl.result.studentCriterias.Count; i++)
            {
                label4.Text += $"\nt{i + 1} = {kraAl.result.studentCriterias[i]}";
            }

            for (int i = 0; i < kraAl.result.paramsDispersion.Count; i++)
            {
                label6.Text += $"\nSb{i} = {kraAl.result.paramsDispersion[i]}";
            }

            alOtherResults.Text += "\n" + $"Оценка дисперсии: {kraAl.result.epsilonSquared}";
            alOtherResults.Text += "\n" + $"Средняя ошибка аппроксимации: {kraAl.result.avgApproximation}%";
            alOtherResults.Text += "\n" + $"Несмещенная оценка дисперсии : {kraAl.result.unshiftedDispersion}";
            alOtherResults.Text += "\n" + $"Среднеквадратическое отклонение: {kraAl.result.avgSquaredShift}";

            kraAl._result.Clear();
        }

        private void calcCraCu()
        {
            List<List<double>> cu = new List<List<double>>();
            List<double> cuResult = readDataGrid2(5);
            corelationCu.Text = "Степень кореляции";
            resultFuncCuLabel.Text = "Результатирующая функция";
            fisherCuLabel.Text = "Критерий Фишера";

            cu.Add(readDataGrid2(0));
            cu.Add(readDataGrid2(1));
            cu.Add(readDataGrid2(2));
            cu.Add(readDataGrid2(3));
            cu.Add(readDataGrid2(4));

            kraCu.setValues(
                cu, cuResult
            );
            kraCu.startKra();
            corelationCu.Text += "\n";

            for (int i = 0; i < kraCu._result.Count; i++)
            {
                if (kraCu._result[i].r >= 0.7)
                {
                    dataGridView4.Columns[i].HeaderCell.Style.BackColor = Color.Green;
                }
                if (kraCu._result[i].r < 0.7 && kraCu._result[i].r >= 0.3)
                {
                    dataGridView4.Columns[i].HeaderCell.Style.BackColor = Color.Yellow;
                }
                if (kraCu._result[i].r < 0.3)
                {
                    dataGridView4.Columns[i].HeaderCell.Style.BackColor = Color.Red;
                }
            }

            for (int j = 0; j < kraCu.finalKoefs.Count; j++)
            {
                if (j == 0)
                {
                    resultFuncCuLabel.Text += " " + kraCu.finalKoefs[0];
                    continue;
                }
                resultFuncCuLabel.Text += $" + {kraCu.finalKoefs[j]} x{j}";
            }

            fisherCuLabel.Text += $": {kraCu.result.fisher}";

            for (int i = 0; i < kraCu.pairCorrelations.Count; i++)
            {
                pairCuGrid.Rows.Add(
                    kraCu.pairCorrelations[i][0],
                    kraCu.pairCorrelations[i][1],
                    kraCu.pairCorrelations[i][2],
                    kraCu.pairCorrelations[i][3],
                    kraCu.pairCorrelations[i][4],
                    kraCu.pairCorrelations[i][5]
                );
                if (i != 5)
                {
                    pairCuGrid.Rows[i].HeaderCell.Value = $"x{i + 1}";
                }
                else
                {
                    pairCuGrid.Rows[i].HeaderCell.Value = "y";
                }
            }

            for (int i = 0; i < kraCu.result.elastics.Count; i++)
            {
                label17.Text += $"\nE{i + 1} = {kraCu.result.elastics[i]}";
            }

            for (int i = 0; i < kraCu.result.studentCriterias.Count; i++)
            {
                label16.Text += $"\nt{i + 1} = {kraCu.result.studentCriterias[i]}";
            }

            for (int i = 0; i < kraCu.result.paramsDispersion.Count; i++)
            {
                label15.Text += $"\nSb{i} = {kraCu.result.paramsDispersion[i]}";
            }

            cuOtherResults.Text += "\n" + $"Оценка дисперсии: {kraCu.result.epsilonSquared}";
            cuOtherResults.Text += "\n" + $"Средняя ошибка аппроксимации: {kraCu.result.avgApproximation}%";
            cuOtherResults.Text += "\n" + $"Несмещенная оценка дисперсии : {kraCu.result.unshiftedDispersion}";
            cuOtherResults.Text += "\n" + $"Среднеквадратическое отклонение: {kraCu.result.avgSquaredShift}";

            kraCu._result.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //try
            //{
            calcKraAl();
            calcCraCu();    

                
            //}
            //catch (Exception exc)
            //{
            //    Console.WriteLine(exc.ToString());    
            //    MessageBox.Show("Некоректный ввод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label13.Text += kraAl.calcYPredict(
                new List<double>() {
                    Convert.ToDouble(alTb1.Text),
                    Convert.ToDouble(alTb2.Text),
                    Convert.ToDouble(alTb3.Text),
                    Convert.ToDouble(alTb4.Text),
                    Convert.ToDouble(alTb5.Text)
                }
            );
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label14.Text += kraCu.calcYPredict(
                new List<double>() {
                    Convert.ToDouble(cuTb1.Text),
                    Convert.ToDouble(cuTb2.Text),
                    Convert.ToDouble(cuTb3.Text),
                    Convert.ToDouble(cuTb4.Text),
                    Convert.ToDouble(cuTb5.Text)
                }
            );
        }
    }
}