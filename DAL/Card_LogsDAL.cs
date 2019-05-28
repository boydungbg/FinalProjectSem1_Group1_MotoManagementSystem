using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class Card_LogsDAL
    {
        private MySqlDataReader reader;
        private string query;
        private MySqlConnection connection;
        public Card_LogsDAL()
        {
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public bool CreateCardLogs(Card_Logs card_Logs)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"insert into Card_Logs(card_id,acc_name,cl_licensePlate,cl_dateTimeStart) values
                ('" + card_Logs.Card_id + "','" + card_Logs.User_name + "','" + card_Logs.LisensePlate + "','" + card_Logs.DateTimeStart?.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs, string licensePlate, string cardid, string dateTimeStart)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Update  Card_logs  SET cl_dateTimeEnd = '" + cardLogs.DateTimeEnd?.ToString("yyyy-MM-dd HH:mm:ss") + "'  , cl_sendTime = '" + cardLogs.SendTime + "',cl_intoMoney = " + cardLogs.IntoMoney + " where card_id = '" + cardid + "'  and cl_licensePlate = '" + licensePlate + "' and cl_dateTimeStart = '" + dateTimeStart + "'; ";
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool DeleteCardLogsByID(string id)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Delete from Card_Logs where card_id ='" + id + "'; ";
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public Card_Logs GetCardLogsByCardIDAndLicensePlate(string cardid, string licensePlate)
        {
            query = @"select card_id,cl_licensePlate,max(cl_dateTimeStart) as cl_dateTimeStart,cl_dateTimeEnd,cl_sendTime,cl_intoMoney from Card_Logs where card_id ='" + cardid + "' and cl_licensePlate='" + licensePlate + "';";
            reader = DBHelper.ExecQuery(query, connection);
            Card_Logs cardLogs = null;
            if (reader.Read())
            {
                cardLogs = GetCardLogsInfo(reader);
            }
            connection.Close();
            return cardLogs;
        }
        private Card_Logs GetCardLogsInfo(MySqlDataReader reader)
        {
            Card_Logs cardLogs = new Card_Logs();
            if (reader.IsDBNull(reader.GetOrdinal("card_id")))
            {
                return null;
            }
            cardLogs.Card_id = reader.GetString("card_id");
            cardLogs.LisensePlate = reader.GetString("cl_licensePlate");
            cardLogs.DateTimeStart = reader.GetDateTime("cl_dateTimeStart");
            if (reader.IsDBNull(reader.GetOrdinal("cl_dateTimeEnd")) && reader.IsDBNull(reader.GetOrdinal("cl_sendTime")) && reader.IsDBNull(reader.GetOrdinal("cl_intoMoney")))
            {
                cardLogs.DateTimeEnd = new DateTime(0);
                cardLogs.SendTime = "Không có";
                cardLogs.IntoMoney = 0;
            }
            else
            {
                cardLogs.DateTimeEnd = reader.GetDateTime("cl_dateTimeEnd");
                cardLogs.SendTime = reader.GetString("cl_sendTime");
                cardLogs.IntoMoney = reader.GetDouble("cl_intoMoney");
            }
            return cardLogs;
        }
        public List<Card_Logs> GetListCardLogs(string from, string to)
        {
            query = @"select * from Card_logs where cl_dateTimeStart between '" + from + "' AND '" + to + "';";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card_Logs> cardLogs = new List<Card_Logs>();
            while (reader.Read())
            {
                cardLogs.Add(GetCardLogsInfo(reader));
            }
            connection.Close();
            return cardLogs;
        }
    }
}