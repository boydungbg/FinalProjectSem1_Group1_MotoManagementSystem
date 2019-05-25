using System;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
namespace DAL
{

    public class DBHelper
    {
        private static MySqlConnection connection;


        public static MySqlConnection GetConnection()
        {
            string connectionString;
            try
            {
                FileStream fileStream = File.OpenRead("ConnectionString.txt");
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    connectionString = reader.ReadLine();
                }
                fileStream.Close();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Lỗi File connectionString.txt");
                return null;
            }
            try
            {
                connection = new MySqlConnection { ConnectionString = connectionString };
                return connection;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        public static MySqlConnection OpenConnection()
        {
            if (connection == null)
            {
                connection = GetConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        public static void CloseConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        public static MySqlDataReader ExecQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            command.CommandText = query;
            return command.ExecuteReader();
        }
        public static void ExecNonQuery(string query)
        {
            OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
            CloseConnection();
        }
        public static bool ExecTransaction(List<string> queries)
        {
            bool result = false;
            OpenConnection();
            MySqlCommand command = connection.CreateCommand();
            MySqlTransaction trans = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = trans;
            try
            {
                foreach (var query in queries)
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                trans.Commit();
                result = true;
            }
            catch (System.Exception e)
            {
                result = false;
                trans.Rollback();
                Console.WriteLine(e);
            }
            finally
            {
                command.CommandText = "unlock tables";
                command.ExecuteNonQuery();
                CloseConnection();
            }

            return result;
        }

    }
}