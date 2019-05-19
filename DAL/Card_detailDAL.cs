using MySql.Data.MySqlClient;
using Persistence;
using System;
namespace DAL
{
    public class Card_detailDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public Card_detailDAL()
        {
            connection = DBHelper.OpenConnection();
        }

        public Card_Detail GetCard_DetailByID(string card_id)
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
                connection.Open();
            }
            MySqlCommand cmd = new MySqlCommand("", connection);
            query = @"Select card_id,cus_id,start_day,end_day,max(date_created) from Card_detail where card_id = @card_id;";
            cmd.Parameters.AddWithValue("@card_id", card_id);
            cmd.CommandText = query;
            Card_Detail card_Detail = null;
            using (reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    card_Detail = GetCard_detail(reader);
                }
            }
            return card_Detail;
        }
        private Card_Detail GetCard_detail(MySqlDataReader reader)
        {
            if (reader.IsDBNull(reader.GetOrdinal("card_id")))
            {
                return null;
            }
            string cardid = reader.GetString("card_id");
            string cusid = reader.GetString("cus_id");
            DateTime startday = reader.GetDateTime("start_day");
            DateTime endday = reader.GetDateTime("end_day");
            DateTime date_created = reader.GetDateTime("max(date_created)");
            Card_Detail card_Detail = new Card_Detail(cardid, cusid, startday, endday, date_created);
            return card_Detail;
        }
    }
}