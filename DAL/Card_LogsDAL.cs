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
        public bool CreateCardLogs(Card_Logs card_Logs)
        {
            if (card_Logs == null)
            {
                return false;
            }
            query = @"insert into Card_Logs(card_id,acc_name,cl_licensePlate,cl_dateTimeStart) values
                ('" + card_Logs.Card_id + "','" + card_Logs.User_name + "','" + card_Logs.LisensePlate + "','" + card_Logs.DateTimeStart?.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            DBHelper.ExecNonQuery(query);
            return true;
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs, string licensePlate, string cardid, string dateTimeStart)
        {
            if (licensePlate == null || cardid == null || dateTimeStart == null || cardLogs == null)
            {
                return false;
            }
            query = @"Update  Card_logs  SET cl_dateTimeEnd = '" + cardLogs.DateTimeEnd?.ToString("yyyy-MM-dd HH:mm:ss") + "'  , cl_sendTime = '" + cardLogs.SendTime + "',cl_intoMoney = " + cardLogs.IntoMoney + " where card_id = '" + cardid + "'  and cl_licensePlate = '" + licensePlate + "' and cl_dateTimeStart = '" + dateTimeStart + "'; ";
            DBHelper.ExecNonQuery(query);
            return true;
        }
        public bool DeleteCardLogsByID(string id)
        {
            if (id == null)
            {
                return false;
            }
            query = @"Delete from Card_Logs where card_id ='" + id + "'; ";
            DBHelper.ExecNonQuery(query);
            return true;
        }
        public Card_Logs GetCardLogsByLicensePlateAndCardID(string licensePlate, string cardid)
        {
            if (licensePlate == null)
            {
                return null;
            }
            query = @"select card_id,cl_licensePlate,max(cl_dateTimeStart),cl_dateTimeEnd,cl_sendTime,cl_intoMoney from Card_Logs where cl_licensePlate ='" + licensePlate + "' and card_id ='" + cardid + "';";
            MySqlCommand cmd = new MySqlCommand(query, DBHelper.OpenConnection());
            Card_Logs cardLogs = null;
            using (reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    cardLogs = new Card_Logs();
                    if (reader.IsDBNull(reader.GetOrdinal("card_id")))
                    {
                        return null;
                    }
                    cardLogs.Card_id = reader.GetString("card_id");
                    cardLogs.LisensePlate = reader.GetString("cl_licensePlate");
                    cardLogs.DateTimeStart = reader.GetDateTime("max(cl_dateTimeStart)");
                    if (reader.IsDBNull(reader.GetOrdinal("cl_dateTimeEnd")) && reader.IsDBNull(reader.GetOrdinal("cl_sendTime")) && reader.IsDBNull(reader.GetOrdinal("cl_intoMoney")))
                    {
                        cardLogs.DateTimeEnd = new DateTime(0);
                        cardLogs.SendTime = "Không có";
                        cardLogs.IntoMoney = 0;
                    }
                }
            }
            DBHelper.CloseConnection();
            return cardLogs;
        }
        private Card_Logs GetCardLogsInfo(MySqlDataReader reader)
        {
            Card_Logs cardLogs = new Card_Logs();
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
            if (from == null || to == null)
            { return null; }
            query = @"select * from Card_logs where cl_dateTimeStart between '" + from + "' and '" + to + "';";
            DBHelper.OpenConnection();
            reader = DBHelper.ExecQuery(query);
            List<Card_Logs> cardLogs = new List<Card_Logs>();
            while (reader.Read())
            {
                cardLogs.Add(GetCardLogsInfo(reader));
            }
            DBHelper.CloseConnection();
            return cardLogs;
        }
    }
}