using System;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class Card_DetailDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public Card_DetailDAL()
        {
            connection = DBHelper.OpenConnection();
        }
        public Card_Detail GetCardDetailByID(string card_id)
        {
            if (card_id == null)
            {
                return null;
            }
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                return null;
            }
            query = $"select * from Card_detail where card_id = '" + card_id + "' and date_created < now();";
            MySqlCommand comman = new MySqlCommand(query, connection);
            Card_Detail card_detail = null;
            using (reader = comman.ExecuteReader())
            {
                if (reader.Read())
                {
                    card_detail = GetCardDetail(reader);
                }
            }
            connection.Close();
            return card_detail;
        }
        public Card_Detail GetCardDetail(MySqlDataReader reader)
        {
            if (reader.GetString("card_id") == null)
            {
                return null;
            }
            string cardid = reader.GetString("card_id");
            string cusid= reader.GetString("cus_id");
            string timecard = reader.GetString("card_time");
            DateTime dateCreated = reader.GetDateTime("date_created");
            Card_Detail card_detail = new Card_Detail(cardid,cusid,timecard,dateCreated);
            return card_detail;
        }
    }
}