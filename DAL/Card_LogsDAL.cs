using System;
using System.Collections.Generic;
using System.Data;
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
            query = @"insert into Card_Logs(card_id,acc_name,cl_licensePlate,cl_timeIn) values
                ('" + card_Logs.Card_id + "','" + card_Logs.User_name + "','" + card_Logs.LisensePlate + "','" + card_Logs.TimeIn?.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs, string licensePlate, int cardid, string timeIn)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Update  Card_logs  SET cl_timeOut = '" + cardLogs.TimeOut?.ToString("yyyy-MM-dd HH:mm:ss") + "', cl_status = " + cardLogs.Status + ",cl_money = " + cardLogs.Money + " where card_id = " + cardid + "  and cl_licensePlate = '" + licensePlate + "' and cl_timeIn = '" + timeIn + "'; ";
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public Card_Logs GetCardLogsByCardIDAndLicensePlate(int cardid, string licensePlate)
        {
            query = @"select card_id,cl_licensePlate,cl_timeIn,cl_timeOut,cl_status,cl_money from Card_Logs where card_id =" + cardid + " and cl_licensePlate='" + licensePlate + "' and cl_status = 0;";
            reader = DBHelper.ExecQuery(query, connection);
            Card_Logs cardLogs = null;
            if (reader.Read())
            {
                cardLogs = GetCardLogsInfo(reader);
            }
            connection.Close();
            return cardLogs;
        }
        public Card_Logs GetCardLogsByLicensePlate(string licensePlate)
        {
            query = @"select card_id,cl_licensePlate,cl_timeIn,cl_timeOut,cl_status,cl_money from Card_Logs where cl_licensePlate='" + licensePlate + "'  and cl_status = 0;";
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
            if (reader.IsDBNull(reader.GetOrdinal("cl_timeOut")))
            {
                cardLogs.TimeOut = new DateTime();
            }
            else
            {
                cardLogs.TimeOut = reader.GetDateTime("cl_timeOut");
            }
            cardLogs.Card_id = reader.GetInt32("card_id");
            cardLogs.TimeIn = reader.GetDateTime("cl_timeIn");
            cardLogs.LisensePlate = reader.GetString("cl_licensePlate");
            cardLogs.Status = reader.GetInt16("cl_status");
            cardLogs.Money = reader.GetDouble("cl_money");
            return cardLogs;
        }
        public List<Card_Logs> GetListCardLogsByPage(int page, string from, string to, string type)
        {
            query = @"call sp_card_logs_statistical(" + page + ",'" + from + "','" + to + "','" + type + "')";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card_Logs> cardLogs = new List<Card_Logs>();
            while (reader.Read())
            {
                cardLogs.Add(GetCardLogsInfo(reader));
            }
            connection.Close();
            return cardLogs;
        }
        public List<Card_Logs> GetListCardLogs(string from, string to, string keyword)
        {
            query = @"select * from Card_logs  where cl_timeIn between '" + from + "' and '" + to + "' and cl_licensePlate like '%" + keyword + "%';";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card_Logs> cardLogs = new List<Card_Logs>();
            while (reader.Read())
            {
                cardLogs.Add(GetCardLogsInfo(reader));
            }
            connection.Close();
            return cardLogs;
        }
        public double GetCardLogNO(string from, string to, string type)
        {
            query = @" select count(cl.cl_id) as cl_id from Card_logs cl inner join Card c on cl.card_id = c.card_id
             where cl.cl_timeIn between '" + from + "' and '" + to + "' and c.card_type = '" + type + "';";
            double CardLogNO = 0;
            reader = DBHelper.ExecQuery(query, connection);
            if (reader.Read())
            {
                CardLogNO = reader.GetInt32("cl_id");
            }
            connection.Close();
            return CardLogNO;
        }
        public Card_Logs GetCardLogsByID(int cardid, int status)
        {
            query = @"select cl_id,cl_status,max(cl_timeIn) from Card_Logs where card_id =" + cardid + " and cl_status = " + status + ";";
            reader = DBHelper.ExecQuery(query, connection);
            Card_Logs cardLogs = new Card_Logs();
            if (reader.Read())
            {
                if (reader.IsDBNull(reader.GetOrdinal("cl_status")))
                {
                    return null;
                }
                cardLogs.Status = reader.GetInt16("cl_status");
            }
            connection.Close();
            return cardLogs;
        }
        public List<Card_Logs> GetListCardLogsByKeyWork(int page, string from, string to, string keyWork)
        {
            query = @"select * from Card_logs  where cl_timeIn between '" + from + "' and '" + to + "' and cl_licensePlate like '%" + keyWork + "%' limit " + page + ",10  ;";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card_Logs> cardLogs = new List<Card_Logs>();
            while (reader.Read())
            {
                cardLogs.Add(GetCardLogsInfo(reader));
            }
            connection.Close();
            return cardLogs;
        }
        public double GetListCardLogsByKeyWorkNo(string from, string to, string keyWord)
        {
            query = @"select count(cl_id) from Card_logs  where cl_timeIn between '" + from + "' and '" + to + "' and cl_licensePlate like '%" + keyWord + "%';";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card_Logs> cardLogs = new List<Card_Logs>();
            double CardLogNO = 0;
            while (reader.Read())
            {
                CardLogNO = reader.GetInt32("count(cl_id)");
            }
            connection.Close();
            return CardLogNO;
        }
    }
}