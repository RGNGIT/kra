using System.Security.Cryptography;
using System.Text;

namespace WinFormsApp1
{
    public partial class Auth : Form
    {
        string[] login = { "admin", "ruk", "oper" };
        string[] pass = { "123", "123", "123" };
        string[] role = { "admin", "rukovoditel", "operator" };
        int N;
        string? hash1;
        string? hash2;
        int id;
        int num;
        int numtrans = 1;
        int counttrans = 10;

        int ResolveIndex(string value, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        public Auth()
        {
            InitializeComponent();
            tabControl2.TabPages[1].Parent = null;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            int i = 0;
            foreach (string log in login)
            {
                dataGridView1.Rows.Add(login[i], pass[i]);
                dataGridView2.Rows.Add(login[i], pass[i]);
                i++;

            }
            i = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            id = 0;
            for (int i = 0; i < login.Length; i++)
            {
                if (textBoxLogin.Text == login[i] && textBoxPassword.Text == pass[i])
                {
                    flag = true;
                    id = i;
                }
            }
            if (flag == true)
            {
                label5.Text = "Ответ: генерация ключа N";
                label6.Text = "Запрос: Ключ аутентификации";
                label5.Visible = true;
                label6.Visible = true;
                Random rnd = new Random();
                N = rnd.Next();
                label7.Text = $"N = {N}";
                label7.Visible = true;

                // label23.Visible = true;
                UserStore.role = role[ResolveIndex(textBoxLogin.Text, login)];
                for (int i = 0; i < 3; i++)
                {
                    Validation();
                }
            }
            else { MessageBox.Show("Ошибка аутентификации: неверный логин или пароль"); }
            flag = false;
        }

        int counter = 0;

        void Validation()
        {
            // string hash1=String.Empty;
            // string hash2=String.Empty; 
            switch (counter)
            {
                case 0:
                    {
                        label5.Text = "Запрос: Захешированный пароль";
                        label6.Text = "Ответ: Генерация хеш-пароля Hash(N,P)";
                        label8.Text = $"N = {N}";
                        label8.Visible = true;
                        // label9.Text = SHA(N+textBox1.Text);
                        label9.Visible = true;
                        string NP = N + textBoxPassword.Text;
                        hash1 = SHA(NP);
                        textBox3.Text = hash1;
                        textBox3.Visible = true;
                        counter++;
                        break;
                    }
                case 1:
                    {

                        label5.Text = "Ответ: Генерация своего хеш-пароля Hash(N,P1). Проверка совпадения Hash(N,P) и Hash(N,P1)";
                        label6.Text = "Запрос: Подтверждение аутентификации";
                        label10.Visible = true;
                        textBoxhash1.Text = hash1;
                        textBoxhash1.Visible = true;

                        string NP1 = N + pass[id];
                        hash2 = SHA(NP1);
                        label11.Visible = true;
                        textBoxhash2.Text = hash2;
                        textBoxhash2.Visible = true;
                        counter++;
                        break;
                    }
                case 2:
                    {
                        if (hash1 == hash2)
                        {
                            label5.Text = "Ответ: Аутентификация пройдена";
                            label6.Text = "Запрос: Подтверждение аутентификации";
                            MessageBox.Show("Аутентификация успешна");
                            Main form1 = new Main();
                            form1.Show();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка аутентификации: неверный логин или пароль");
                        }
                        counter = 0;
                        break;

                    }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Validation();
        }
        public static string SHA(string input)
        {
            SHA256 hash = SHA256.Create();
            return BitConverter.ToString(hash.ComputeHash(Encoding.ASCII.GetBytes(input)));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = false;
            num = 0;
            for (int i = 0; i < login.Length; i++)
            {

            }
            foreach (string log in login)
            {
                if (textBox6.Text == log)
                {
                    flag = true;
                    break;
                }
                num++;


            }
            if (flag == true)
            {
                label14.Text = "Сервер";
                label15.Text = "Пользователь";
                label14.Visible = true;
                label15.Visible = true;
                string k1 = pass[num];
                for (int i = 0; i < counttrans; i++)
                {
                    k1 = SHA(k1);
                    dataGridView3.Rows.Add(i + 1, k1);
                }
                string k2 = textBox7.Text;
                for (int i = 0; i < counttrans; i++)
                {
                    k2 = SHA(k2);
                    dataGridView4.Rows.Add(i + 1, k2);
                }
                dataGridView3.Visible = true;
                dataGridView4.Visible = true;
                label16.Visible = true;
                label17.Visible = true;
                textBox8.Visible = true;
                textBox9.Visible = true;
                button4.Visible = true;
                label20.Text = $"Номер транзакции {numtrans}";
                label18.Text = $"Ключ пользователя для {numtrans} транзакции";
                label19.Text = $"Ключ сервера для {numtrans} транзакции";
                label20.Visible = true;
                label18.Visible = true;
                // label19.Visible = true;
                textBox10.Text = Convert.ToString(dataGridView3.Rows[counttrans - numtrans - 1].Cells[1].Value);
                textBox10.Visible = true;
                label21.Text = $"Ключ полученный после хеширования ключа переданного пользователем ";
            }
            else { MessageBox.Show("Ошибка аутентификации: неверный логин или пароль"); }
            flag = false;
        }
        int count;
        private void button5_Click(object sender, EventArgs e)
        {
            switch (count)
            {
                case 0:
                    {
                        label20.Text = $"Номер транзакции {numtrans}";
                        label18.Text = $"Ключ пользователя для {numtrans} транзакции";
                        label19.Text = $"Ключ сервера для {numtrans} транзакции";
                        label20.Visible = true;
                        label18.Visible = true;
                        label19.Visible = true;
                        textBox10.Text = Convert.ToString(dataGridView3.Rows[counttrans - numtrans - 1].Cells[1].Value);
                        textBox10.Visible = true;
                        textBox11.Text = Convert.ToString(dataGridView4.Rows[counttrans - numtrans].Cells[1].Value);
                        textBox11.Visible = true;

                        count++;
                        break;
                    }
                case 1:
                    {


                        label21.Visible = true;
                        textBox12.Text = SHA(textBox9.Text);
                        textBox12.Visible = true;


                        count++;
                        break;
                    }
                case 2:
                    {
                        if (textBox12.Text == textBox11.Text)
                        {
                            textBox11.Text = textBox12.Text;
                            textBox12.Clear();
                            numtrans = numtrans + 1;
                            label20.Text = $"Номер транзакции {numtrans}";
                            label18.Text = $"Ключ пользователя для {numtrans} транзакции";
                            textBox10.Text = Convert.ToString(dataGridView3.Rows[counttrans - numtrans - 1].Cells[1].Value);
                            button5.Visible = false;
                            textBox9.Clear();
                            MessageBox.Show("Аутентификация успешна");
                        }
                        else
                        {
                            textBox9.Clear();
                            MessageBox.Show("Ошибка аутентификации: неверный логин или пароль");
                        }
                        count = 0;
                        break;

                    }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button5.Visible = true;
        }
    }
}
