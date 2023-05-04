using System;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{
    public partial class Main : Form
    {

        string connection = "Server=AORUS;Integrated Security=true;";

        public Main()
        {
            InitializeComponent();
            labelDebug.Text = UserStore.role;
            switch (UserStore.role)
            {
                case "operator":
                    tabControMain.TabPages[2].Parent = null;
                    break;
                case "rukovoditel":
                    // Все вкладки (кроме введения?)
                    break;
                case "admin":
                    // Все вкладки
                    break;
            }
            this.toggleLabels(false);
            /*
            dataGridView3.Rows.Add(-12, 2500, 2450, 50000, 5, 200);
            dataGridView3.Rows.Add(-13, 2500, 3500, 0, 0, 125);
            dataGridView3.Rows.Add(-5, 3550, 2525, 32000, 10, 150);
            dataGridView3.Rows.Add(7, 2500, 2500, 41000, 10, 220);
            dataGridView3.Rows.Add(16, 3550, 4600, 5000, 15, 260);
            dataGridView3.Rows.Add(20, 2450, 2500, 22000, 0, 230);
            dataGridView3.Rows.Add(22, 2500, 2550, 40000, 5, 280);
            
            dataGridViewFactCopper.Rows.Add(-12, 2500, 2450, 50000, 5, 200);
            dataGridViewFactCopper.Rows.Add(-13, 2500, 3500, 0, 0, 125);
            dataGridViewFactCopper.Rows.Add(-5, 3550, 2525, 32000, 10, 150);
            dataGridViewFactCopper.Rows.Add(7, 2500, 2500, 41000, 10, 220);
            dataGridViewFactCopper.Rows.Add(16, 3550, 4600, 5000, 15, 260);
            dataGridViewFactCopper.Rows.Add(20, 2450, 2500, 22000, 0, 230);
            dataGridViewFactCopper.Rows.Add(22, 2500, 2550, 40000, 5, 280);
            */
            RefreshAll();
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

            for (int rows = 0; rows < dataGridViewFactAlum.Rows.Count - 1; rows++)
            {
                try
                {
                    data.Add(Convert.ToDouble(dataGridViewFactAlum.Rows[rows].Cells[index].Value));
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

            for (int rows = 0; rows < dataGridViewFactCopper.Rows.Count - 1; rows++)
            {
                try
                {
                    data.Add(Convert.ToDouble(dataGridViewFactCopper.Rows[rows].Cells[index].Value));
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
                    dataGridViewFactAlum.Columns[i].HeaderCell.Style.BackColor = Color.Green;
                }
                if (kraAl._result[i].r < 0.7 && kraAl._result[i].r >= 0.3)
                {
                    dataGridViewFactAlum.Columns[i].HeaderCell.Style.BackColor = Color.Yellow;
                }
                if (kraAl._result[i].r < 0.3)
                {
                    dataGridViewFactAlum.Columns[i].HeaderCell.Style.BackColor = Color.Red;
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
                    dataGridViewFactCopper.Columns[i].HeaderCell.Style.BackColor = Color.Green;
                }
                if (kraCu._result[i].r < 0.7 && kraCu._result[i].r >= 0.3)
                {
                    dataGridViewFactCopper.Columns[i].HeaderCell.Style.BackColor = Color.Yellow;
                }
                if (kraCu._result[i].r < 0.3)
                {
                    dataGridViewFactCopper.Columns[i].HeaderCell.Style.BackColor = Color.Red;
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

        void PostPrognoz(string value, string factTuple, string planTemperature, string planPrice, string planConcPrice, string planAdPrice, string planDiscount, string productId)
        {
            DBWorks works1 = new DBWorks(connection);
            works1.InsertPlan(planTemperature, planPrice, planConcPrice, planAdPrice, planDiscount, productId);
            DBWorks works2 = new DBWorks(connection);
            dataGridViewBuffer.DataSource = works2.ReturnTable("[номер_плана]", "[VKR].[dbo].[План_выпуска]", null);
            DBWorks works3 = new DBWorks(connection);
            works3.InsertPrognoz(value, factTuple, dataGridViewBuffer.Rows[dataGridViewBuffer.Rows.Count - 2].Cells[0].Value.ToString()!);
        }

        string ResolveTuple(DataGridView grid)
        {
            string temp = "";
            for (int i = 0; i < grid.Rows.Count - 1; i++)
            {
                temp += grid.Rows[i].Cells[6].Value.ToString() + (i != grid.Rows.Count - 2 ? ',' : "");
            }
            return temp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double result = kraAl.calcYPredict(
                new List<double>() {
                    Convert.ToDouble(alTb1.Text),
                    Convert.ToDouble(alTb2.Text),
                    Convert.ToDouble(alTb3.Text),
                    Convert.ToDouble(alTb4.Text),
                    Convert.ToDouble(alTb5.Text)
                }
            );
            label13.Text += result;
            PostPrognoz(result.ToString(), ResolveTuple(dataGridViewFactAlum), alTb1.Text, alTb2.Text, alTb3.Text, alTb4.Text, alTb5.Text, "5");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double result = kraCu.calcYPredict(
                new List<double>() {
                    Convert.ToDouble(cuTb1.Text),
                    Convert.ToDouble(cuTb2.Text),
                    Convert.ToDouble(cuTb3.Text),
                    Convert.ToDouble(cuTb4.Text),
                    Convert.ToDouble(cuTb5.Text)
                }
            );
            label14.Text += result;
            PostPrognoz(result.ToString(), ResolveTuple(dataGridViewFactCopper), cuTb1.Text, cuTb2.Text, cuTb3.Text, cuTb4.Text, cuTb5.Text, "1");
        }

        void SetProductionFacts()
        {
            DBWorks works1 = new DBWorks(connection);
            // Аллюм
            dataGridViewFactAlum.DataSource = works1.ReturnTable(
                "c.[темп_окр_среды] AS 'Температура окружающей среды', c.[цена] AS 'Цена', c.[цена_конкурентов] AS 'Цена конкурентов', c.[цена_на_рекламу] AS 'Цена на рекламу', c.[скидка] AS 'Скидка', c.[количество_проданных] AS 'Количество выпущенных', c.[номер_факта_выпуска] ",
                "[VKR].[dbo].[Выпускаемая_продукция] AS a, [VKR].[dbo].[Вид_выпускаемой_продукции] AS b, [VKR].[dbo].[Факт_выпуска] AS c",
                "WHERE a.[код_вида] = b.[код_вида] AND c.[код_выпускаемой_продукции] = a.[номер_выпускаемой_продукции] AND b.[код_вида] = 9;"
            );
            DBWorks works2 = new DBWorks(connection);
            // Медь
            dataGridViewFactCopper.DataSource = works2.ReturnTable(
                "c.[темп_окр_среды] AS 'Температура окружающей среды', c.[цена] AS 'Цена', c.[цена_конкурентов] AS 'Цена конкурентов', c.[цена_на_рекламу] AS 'Цена на рекламу', c.[скидка] AS 'Скидка', c.[количество_проданных] AS 'Количество выпущенных', c.[номер_факта_выпуска] ",
                "[VKR].[dbo].[Выпускаемая_продукция] AS a, [VKR].[dbo].[Вид_выпускаемой_продукции] AS b, [VKR].[dbo].[Факт_выпуска] AS c",
                "WHERE a.[код_вида] = b.[код_вида] AND c.[код_выпускаемой_продукции] = a.[номер_выпускаемой_продукции] AND b.[код_вида] = 8;"
            );
        }

        void SetBufferCombo()
        {
            DBWorks works = new DBWorks(connection);
            dataGridViewBuffer.DataSource = works.ReturnTable(
                "a.[номер_выпускаемой_продукции], a.[наименование_выпускаемой_продукции], b.[наименование_вида]",
                "[VKR].[dbo].[Выпускаемая_продукция] AS a, [VKR].[dbo].[Вид_выпускаемой_продукции] AS b",
                "WHERE a.[код_вида] = b.[код_вида];"
            );
        }

        void UpdateProductionCombo()
        {
            DBWorks works = new DBWorks(connection);
            dataGridViewBuffer.DataSource = works.ReturnTable(
                "a.[номер_выпускаемой_продукции], a.[наименование_выпускаемой_продукции], b.[наименование_вида]",
                "[VKR].[dbo].[Выпускаемая_продукция] AS a, [VKR].[dbo].[Вид_выпускаемой_продукции] AS b",
                "WHERE a.[код_вида] = b.[код_вида];"
            );
            comboBoxFactProduct.Items.Clear();
            for (int i = 0; i < dataGridViewBuffer.Rows.Count - 1; i++)
            {
                comboBoxFactProduct.Items.Add($"{dataGridViewBuffer.Rows[i].Cells[1].Value} {dataGridViewBuffer.Rows[i].Cells[2].Value}");
            }
        }

        void UpdatePrognozCombo()
        {
            comboBoxPrognozes.Items.Clear();
            DBWorks works = new DBWorks(connection);
            dataGridViewBuffer.DataSource = works.ReturnTable("[код_прогноза], [значение], [кортеж_фактов], [код_плана]", "[VKR].[dbo].[Прогноз]", null);
            for (int i = 0; i < dataGridViewBuffer.Rows.Count - 1; i++)
            {
                comboBoxPrognozes.Items.Add($"Прогноз {dataGridViewBuffer.Rows[i].Cells[0].Value}");
            }
        }

        string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        void RefreshAll()
        {
            DBWorks works1 = new DBWorks(connection);
            UpdateProductionCombo();
            if (tabControMain.SelectedIndex == 0)
            {
                switch (tabControlDataworks.SelectedIndex)
                {
                    case 0:
                        dataGridViewFact.DataSource = works1.ReturnTable(
                            "a.[темп_окр_среды], a.[цена], a.[цена_конкурентов], a.[цена_на_рекламу], a.[скидка], a.[количество_проданных], " +
                            "b.[наименование_выпускаемой_продукции]",
                            "[VKR].[dbo].[Факт_выпуска] AS a, [VKR].[dbo].[Выпускаемая_продукция] AS b",
                            "WHERE a.[код_выпускаемой_продукции] = b.[номер_выпускаемой_продукции];"
                        );
                        break;
                }
            }
            DBWorks works2 = new DBWorks(connection);
            if (tabControMain.SelectedIndex == 1)
            {
                SetProductionFacts();
                dataGridViewBuffer.DataSource = works2.ReturnTable(
                    "a.[темп_окр_среды], a.[цена], a.[цена_конкурентов], a.[цена_на_рекламу], a.[скидка], a.[количество_проданных], " +
                    "c.[наименование_вида]",
                    "[VKR].[dbo].[Факт_выпуска] AS a, [VKR].[dbo].[Выпускаемая_продукция] AS b, [VKR].[dbo].[Вид_выпускаемой_продукции] AS c",
                    "WHERE a.[код_выпускаемой_продукции] = b.[номер_выпускаемой_продукции] AND b.[код_вида] = c.[код_вида];"
                );
            }
            if (tabControMain.SelectedIndex == 3)
            {
                UpdatePrognozCombo();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private void buttonAddFact_Click(object sender, EventArgs e)
        {
            DBWorks works = new DBWorks(connection);
            SetBufferCombo();
            works.InsertFact(
                textBoxFactTempOrkSred.Text,
                textBoxFactPrice.Text,
                textBoxFactConcurentPrice.Text,
                textBoxFactAdPrice.Text,
                textBoxFactDiscount.Text,
                textBoxFactAmountSold.Text,
                dataGridViewBuffer.Rows[comboBoxFactProduct.SelectedIndex].Cells[0].Value.ToString()!
            );
            UpdateProductionCombo();
            RefreshAll();
        }

        private void comboBoxPrognozes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DBWorks works1 = new DBWorks(connection);
            string[] prognozId = comboBoxPrognozes.Text.Split(' ');
            dataGridViewBuffer.DataSource = works1.ReturnTable(
                "*",
                "[VKR].[dbo].[Прогноз] AS a, [VKR].[dbo].[План_выпуска] AS b",
                $"WHERE a.[код_плана] = b.[номер_плана] AND a.[код_прогноза] = {prognozId[1]};"
            );
            labelStatValue.Text = "Y = " + dataGridViewBuffer.Rows[0].Cells[1].Value.ToString();
            string planId = dataGridViewBuffer.Rows[0].Cells[3].Value.ToString()!;
            string[] factTuple = dataGridViewBuffer.Rows[0].Cells[2].Value.ToString()!.Split(',');
            DBWorks works2 = new DBWorks(connection);
            dataGridViewBuffer.DataSource = works2.ReturnTable(
                "*",
                "[VKR].[dbo].[План_выпуска] AS a",
                $"WHERE a.[номер_плана] = {planId};"
            );
            labelStatTemp.Text = $"Плановая температура окружающей среды: {dataGridViewBuffer.Rows[0].Cells[1].Value}";
            labelStatPrice.Text = $"Плановая цена: {dataGridViewBuffer.Rows[0].Cells[2].Value}";
            labelStatsConcurrent.Text = $"Плановая цена конкурентов: {dataGridViewBuffer.Rows[0].Cells[3].Value}";
            labelStatsAdPrice.Text = $"Плановая цена рекламы: {dataGridViewBuffer.Rows[0].Cells[4].Value}";
            labelStatsDiscount.Text = $"Плановая скидка: {dataGridViewBuffer.Rows[0].Cells[5].Value}";
            string productId = dataGridViewBuffer.Rows[0].Cells[6].Value.ToString()!;
            DBWorks works3 = new DBWorks(connection);
            dataGridViewBuffer.DataSource = works3.ReturnTable("*", "[VKR].[dbo].[Выпускаемая_продукция] AS a", $"WHERE a.[номер_выпускаемой_продукции] = {productId};");
            labelStatProdName.Text = dataGridViewBuffer.Rows[0].Cells[1].Value.ToString()!;
            dataGridViewStats.Rows.Clear();
            foreach (string factId in factTuple)
            {
                DBWorks works4 = new DBWorks(connection);
                dataGridViewBuffer.DataSource = works4.ReturnTable("*", "[VKR].[dbo].[Факт_выпуска] AS a", $"WHERE a.[номер_факта_выпуска] = {factId};");
                dataGridViewStats.Rows.Add(
                    dataGridViewBuffer.Rows[0].Cells[1].Value,
                    dataGridViewBuffer.Rows[0].Cells[2].Value,
                    dataGridViewBuffer.Rows[0].Cells[3].Value,
                    dataGridViewBuffer.Rows[0].Cells[4].Value,
                    dataGridViewBuffer.Rows[0].Cells[5].Value,
                    dataGridViewBuffer.Rows[0].Cells[6].Value
                );
            }
        }
    }
}