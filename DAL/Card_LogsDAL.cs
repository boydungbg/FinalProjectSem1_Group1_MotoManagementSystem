using System;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class Card_LogsDAL
    {
        private MySqlConnection connection;

        private MySqlDataReader reader;
        private string query;

        public Card_LogsDAL()
        {
            connection = DBHelper.OpenConnection();
        }
        public bool CreateCardLogs(Card_Logs card_Logs)
        {
            bool result = false;
            if (card_Logs == null)
            {
                return result;
            }
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand command = new MySqlCommand("", connection);
            //Lock table 
            command.CommandText = @"lock tables Card_Logs write;";
            command.ExecuteNonQuery();
            MySqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                query = @"insert into Card_Logs(card_id,acc_name,cl_licensePlate,cl_dateTimeStart) values
                (@cardid,@accname,@licensePlate,@dateTimeStart)";
                command.Parameters.AddWithValue("@cardid", card_Logs.Card_id);
                command.Parameters.AddWithValue("@accname", card_Logs.User_name);
                command.Parameters.AddWithValue("@licensePlate", card_Logs.LisensePlate);
                command.Parameters.AddWithValue("@dateTimeStart", card_Logs.DateTimeStart);
                command.CommandText = query;
                command.ExecuteNonQuery();
                transaction.Commit();
                result = true;
                return result;
            }
            catch (System.Exception e)
            {
                string m = e.Message;
                transaction.Rollback();
                throw;
            }
            finally
            {
                command.CommandText = "unlock tables";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public bool DeleteCardLogsByID(string id)
        {
            if (id == null)
            {
                return false;
            }
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand("", connection);
            query = @"Delete from Card_Logs where card_id = @cardid; ";
            cmd.Parameters.AddWithValue("@cardid", id);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            return true;
        }
        public Card_Logs GetCardLogsByLicensePlate(string licensePlate)
        {
            if (licensePlate == null)
            {
                return null;
            }
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"select card_id,cl_licensePlate,max(cl_dateTimeStart) from Card_Logs where cl_licensePlate = @licensePlate;";
            command.Parameters.AddWithValue("@licensePlate", licensePlate);
            command.CommandText = query;
            Card_Logs cardLogs = null;
            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cardLogs = GetCardLogs(reader);
                }
            }
            return cardLogs;
        }
        private Card_Logs GetCardLogs(MySqlDataReader reader)
        {
            if (reader.IsDBNull(reader.GetOrdinal("card_id")))
            {
                return null;
            }
            string cardid = reader.GetString("card_id");
            string licensePlate = reader.GetString("cl_licensePlate");
            DateTime dateTimeStart = reader.GetDateTime("max(cl_dateTimeStart)");
            Card_Logs cardLogs = new Card_Logs(cardid, null, licensePlate, dateTimeStart, null, null, null);
            return cardLogs;
        }


    }
}