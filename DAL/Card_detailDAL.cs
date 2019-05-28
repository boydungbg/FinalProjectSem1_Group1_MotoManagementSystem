using MySql.Data.MySqlClient;
using Persistence;
using System;
namespace DAL
{
    public class Card_detailDAL
    {
        private MySqlDataReader reader;
        private string query;
        private MySqlConnection connection;
        public Card_detailDAL()
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
        public Card_Detail GetCard_DetailByID(string card_id)
        {
            query = @"Select card_id,cus_id,start_day,max(end_day) from Card_detail where card_id = '" + card_id + "' ;";
            reader = DBHelper.ExecQuery(query, connection);
            Card_Detail card_Detail = null;
            if (reader.Read())
            {
                card_Detail = GetCard_detailInfo(reader);
            }
            connection.Close();
            return card_Detail;
        }
        private Card_Detail GetCard_detailInfo(MySqlDataReader reader)
        {
            Card_Detail card_Detail = new Card_Detail();
            if (reader.IsDBNull(reader.GetOrdinal("card_id")))
            {
                return null;
            }
            card_Detail.Card_id = reader.GetString("card_id");
            card_Detail.Cus_id = reader.GetString("cus_id");
            card_Detail.Start_day = reader.GetDateTime("start_day");
            card_Detail.End_day = reader.GetDateTime("max(end_day)");
            return card_Detail;
        }
    }
}