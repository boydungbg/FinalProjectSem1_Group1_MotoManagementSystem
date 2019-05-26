using MySql.Data.MySqlClient;
using Persistence;
using System;
namespace DAL
{
    public class Card_detailDAL
    {
        private MySqlDataReader reader;
        private string query;
        public Card_Detail GetCard_DetailByID(string card_id)
        {
            if (card_id == null)
            {
                return null;
            }
            query = @"Select card_id,cus_id,start_day,max(end_day) from Card_detail where card_id = '" + card_id + "' ;";
            DBHelper.OpenConnection();
            Card_Detail card_Detail = null;
            reader = DBHelper.ExecQuery(query);
            if (reader.Read())
            {
                card_Detail = GetCard_detailInfo(reader);
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
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