﻿using System;
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

        public string InsertFact(string temperature, string price, string concPrice, string adPrice, string discount, string soldAmount, string productKey, string month, string year, string plantId)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"INSERT INTO [VKR].[dbo].[Факт_выпуска] ([темп_окр_среды], [цена], [цена_конкурентов], [цена_на_рекламу], [скидка], [количество_проданных], [код_выпускаемой_продукции], [месяц], [год], [код_предприятия]) " +
                    $"VALUES ('{temperature}', '{price}', '{concPrice}', '{adPrice}', '{discount}', '{soldAmount}', '{productKey}', '{month}', '{year}', '{plantId}');", 
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

        public string InsertAlternative(string value)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO [VKR].[dbo].[Подбор_оптимальной_альтернативы] ([наименование_вида]) VALUES ('{value}');", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string InsertPrognoz(string value, string factTuple, string planId, string branchId)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO [VKR].[dbo].[Прогноз] ([значение], [список_фактов], [код_плана], [код_отдела]) VALUES ('{value}', '{factTuple}', '{planId}', '{branchId}');", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string InsertPlant(string name)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO [VKR].[dbo].[Предприятие] ([название]) VALUES ('{name}');", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string InsertBranch(string name, string plantId)
        {
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO [VKR].[dbo].[Отдел] ([наименование], [код_предприятия]) VALUES ('{name}', '{plantId}');", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateFact(string temperature, string price, string concPrice, string adPrice, string discount, string soldAmount, string productKey, string month, string year, string plantId, string factId)
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    $"UPDATE [VKR].[dbo].[Факт_выпуска] SET [темп_окр_среды] = '{temperature}', [цена] = '{price}', [цена_конкурентов] = '{concPrice}', [цена_на_рекламу] = '{adPrice}', [скидка] = '{discount}', [количество_проданных] = '{soldAmount}', [код_выпускаемой_продукции] = '{productKey}', [месяц] = '{month}', [код_предприятия] = '{plantId}', [год] = '{year}' " +
                    $"WHERE [номер_факта_выпуска] = {factId};",
                    connection
                );
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdatePlant(string name, string plantId)
        {
            try
            {
                SqlCommand command = new SqlCommand($"UPDATE [VKR].[dbo].[Предприятие] SET [название] = '{name}' WHERE [код_предприятия] = {plantId};", connection);
                command.ExecuteNonQuery();
                connection.Close();
                return "Команда выполнена";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateBranch(string name, string plantId, string branchId)
        {
            try
            {
                SqlCommand command = new SqlCommand($"UPDATE [VKR].[dbo].[Отдел] SET [наименование] = '{name}', [код_предприятия] = '{plantId}' WHERE [код_отдела] = {branchId};", connection);
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
