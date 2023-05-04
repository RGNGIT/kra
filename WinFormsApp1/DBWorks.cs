using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace WinFormsApp1
{
    internal class DBWorks
    {
        SqlConnection connection;

        public DBWorks(string credentials)
        {
            connection = new SqlConnection(credentials);
            connection.Open();
        }

        public DataView ReturnTable(string Columns, string TablesName, string? Arguments)
        {
            SqlDataAdapter sqlData = new SqlDataAdapter($"SELECT {Columns} FROM {TablesName} {Arguments};", this.connection);
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);
            connection.Close();
            return dataSet.Tables[0].DefaultView;
        }

        public string InsertFact(string temperature, string price, string concPrice, string adPrice, string discount, string soldAmount, string productKey, string month)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO [VKR].[dbo].[Факт_выпуска] ([темп_окр_среды], [цена], [цена_конкурентов], [цена_на_рекламу], [скидка], [количество_проданных], [код_выпускаемой_продукции], [месяц]) " +
                    $"VALUES ('{temperature}', '{price}', '{concPrice}', '{adPrice}', '{discount}', '{soldAmount}', '{productKey}', '{month}');", 
                    connection
                );
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            } 
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }

        public string InsertPlan(string planTemperature, string planPrice, string planConcPrice, string planAdPrice, string planDiscount, string productId)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO [VKR].[dbo].[План_выпуска] ([темп_окр_среды], [цена], [цена_конкурентов], [цена_на_рекламу], [скидка], [код_выпускаемой_продукции]) " +
                    $"VALUES ('{planTemperature}', '{planPrice}', '{planConcPrice}', '{planAdPrice}', '{planDiscount}', '{productId}');", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            } 
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }

        public string InsertPrognoz(string value, string factTuple, string planId)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO [VKR].[dbo].[Прогноз] ([значение], [кортеж_фактов], [код_плана]) VALUES ('{value}', '{factTuple}', '{planId}');", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
