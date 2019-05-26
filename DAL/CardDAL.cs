using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CardDAL
    {
        private MySqlDataReader reader;
        private string query;
        public bool CreateCard(Card card, Customer cus, Card_Detail card_Detail)
        {
            bool result = false;
            if (card == null || cus == null || card_Detail == null)
            {
                return result;
            }
            List<String> queries = new List<string>();
            //Lock table 
            query = @"lock tables Customer write, Card write, Card_detail write;";
            DBHelper.OpenConnection();
            DBHelper.ExecNonQuery(query);
            DBHelper.CloseConnection();
            // Insert Customer
            queries.Add(@"insert into Customer(cus_id,cus_fullname,cus_address,license_plate)
                values   ('" + cus.Cus_id + "','" + cus.Cus_name + "','" + cus.Cus_address + "','" + cus.Cus_licensePlate + "');");
            //Insert card
            queries.Add(@"insert into Card(card_id,card_type,license_plate) 
                values  ('" + card.Card_id + "','" + card.Card_type + "','" + cus.Cus_licensePlate + "');");
            // Insert Card_detail
            queries.Add(@"insert into Card_detail (card_id,cus_id,start_day,end_day)
                        values ('" + card.Card_id + "','" + cus.Cus_id + "','" + card_Detail.Start_day?.ToString("yyyy-MM-dd HH:mm:ss") + "','" + card_Detail.End_day?.ToString("yyyy-MM-dd HH:mm:ss")
                        + "');");
            //Transaction
            result = DBHelper.ExecTransaction(queries);
            return result;
        }
        public bool UpdateCardByID(Card card, string cardid)
        {

            if (card == null || cardid == null)
            {
                return false;
            }
            query = @"Update Card SET license_plate = '" + card.LicensePlate + "', card_status = '" + card.Card_Status + "' where  card_id = '" + cardid + "'; ";
            DBHelper.OpenConnection();
            DBHelper.ExecNonQuery(query);
            DBHelper.CloseConnection();
            return true;
        }
        public bool DeleteCardByID(string cardid, string cusid)
        {
            if (cardid == null || cusid == null)
            {
                return false;
            }
            DBHelper.OpenConnection();
            query = @"Delete from Card_detail where card_id = '" + cardid + "' and cus_id = '" + cusid + "';";
            DBHelper.ExecNonQuery(query);
            query = @"Delete from Customer where cus_id = '" + cusid + "';";
            DBHelper.ExecNonQuery(query);
            query = @"Delete from Card where card_id = '" + cardid + "';";
            DBHelper.ExecNonQuery(query);
            DBHelper.CloseConnection();
            return true;
        }
        public Card GetCardByID(string cardid)
        {
            if (cardid == null)
            {
                return null;
            }
            query = @"select * from Card where card_id = '" + cardid + "' ;";
            DBHelper.OpenConnection();
            reader = DBHelper.ExecQuery(query);
            Card card = null;
            if (reader.Read())
            {
                card = GetCardInfo(reader);
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
            return card;
        }
        public Card GetCardByLicensePlate(string licensePlate)
        {
            if (licensePlate == null)
            {
                return null;
            }
            query = @"select * from Card where license_plate = '" + licensePlate + "' ;";
            DBHelper.OpenConnection();
            reader = DBHelper.ExecQuery(query);
            Card card = null;
            if (reader.Read())
            {
                card = GetCardInfo(reader);
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
            return card;
        }
        private Card GetCardInfo(MySqlDataReader reader)
        {
            Card card = new Card();
            if (reader.IsDBNull(reader.GetOrdinal("card_id")))
            {
                return null;
            }
            card.Card_id = reader.GetString("card_id");
            card.LicensePlate = reader.GetString("license_plate");
            card.Card_type = reader.GetString("card_type");
            card.Card_Status = reader.GetInt32("card_status");
            card.Date_created = reader.GetDateTime("date_created");
            return card;
        }
        public List<Card> GetlistCard()
        {
            query = @"select * from Card;";
            DBHelper.OpenConnection();
            reader = DBHelper.ExecQuery(query);
            List<Card> card = new List<Card>();
            while (reader.Read())
            {
                card.Add(GetCardInfo(reader));
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
            return card;
        }
        public Card GetCardByWord()
        {
            query = @"SELECT max(card_id) from Card where card_id like 'CM%' ;";
            DBHelper.OpenConnection();
            reader = DBHelper.ExecQuery(query);
            Card card = null;
            if (reader.Read())
            {
                if (reader.IsDBNull(reader.GetOrdinal("max(card_id)")))
                {
                    return null;

                }
                card = new Card();
                card.Card_id = reader.GetString("max(card_id)");
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
            return card;
        }
    }
}