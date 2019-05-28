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
        private MySqlConnection connection;
        public CardDAL()
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
        public bool CreateCard(Card card, Customer cus, Card_Detail card_Detail)
        {
            bool result = false;
            MySqlCommand command = new MySqlCommand("", connection);
            //Lock table 
            command.CommandText = @"lock tables Customer write, Card write, Card_detail write;";
            command.ExecuteNonQuery();
            //Transaction
            MySqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                //Insert card
                command.CommandText = @"insert into Card(card_id,card_type,license_plate) 
                values  (@cardid,@cardType,@LicensePlate);";
                command.Parameters.AddWithValue("@cardid", card.Card_id);
                command.Parameters.AddWithValue("@cardType", card.Card_type);
                command.Parameters.AddWithValue("@LicensePlate", cus.Cus_licensePlate);
                command.ExecuteNonQuery();
                // Insert Customer
                command.CommandText = @"insert into Customer(cus_id,cus_fullname,cus_address,license_plate)
                values   (@cus_id,@cus_name,@cus_address,@license_Plate);";
                command.Parameters.AddWithValue("@cus_id", cus.Cus_id);
                command.Parameters.AddWithValue("@cus_name", cus.Cus_name);
                command.Parameters.AddWithValue("@cus_address", cus.Cus_address);
                command.Parameters.AddWithValue("@license_Plate", cus.Cus_licensePlate);
                command.ExecuteNonQuery();
                // Insert Card_detail
                command.CommandText = @"insert into Card_detail (card_id,cus_id,start_day,end_day)
                        values (@card_id_card_detail,@cus_id_card_detail,@start_day,@end_day);";
                command.Parameters.AddWithValue("@card_id_card_detail", card.Card_id);
                command.Parameters.AddWithValue("@cus_id_card_detail", cus.Cus_id);
                command.Parameters.AddWithValue("@start_day", card_Detail.Start_day);
                command.Parameters.AddWithValue("@end_day", card_Detail.End_day);
                command.ExecuteNonQuery();
                transaction.Commit();
                result = true;
            }
            catch (System.Exception e)
            {
                string m = e.Message;
                Console.WriteLine(m);
                transaction.Rollback();
            }
            finally
            {
                // Unlock Tables
                command.CommandText = "unlock tables";
                command.ExecuteNonQuery();
                connection.Close();
            }
            return result;
        }
        public bool UpdateCardByID(Card card, string cardid)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Update Card SET license_plate = '" + card.LicensePlate + "', card_status = '" + card.Card_Status + "' where  card_id = '" + cardid + "'; ";
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool DeleteCardByID(string cardid, string cusid)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Delete from Card_detail where card_id = @card_id_card_detail and cus_id = @cus_id_card_detail;";
            command.Parameters.AddWithValue("@card_id_card_detail", cardid);
            command.Parameters.AddWithValue("@cus_id_card_detail", cusid);
            command.CommandText = query;
            command.ExecuteNonQuery();
            query = @"Delete from Customer where cus_id = @cusid;";
            command.Parameters.AddWithValue("@cusid", cusid);
            command.CommandText = query;
            command.ExecuteNonQuery();
            query = @"Delete from Card where card_id = @cardid;";
            command.Parameters.AddWithValue("@cardid", cardid);
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public Card GetCardByID(string cardid)
        {
            query = @"select * from Card where card_id = '" + cardid + "' ;";
            reader = DBHelper.ExecQuery(query, connection);
            Card card = null;
            if (reader.Read())
            {
                card = GetCardInfo(reader);
            }
            connection.Close();
            return card;
        }
        public Card GetCardByLicensePlate(string licensePlate)
        {
            query = @"select * from Card where license_plate = '" + licensePlate + "' ;";
            reader = DBHelper.ExecQuery(query, connection);
            Card card = null;
            if (reader.Read())
            {
                card = GetCardInfo(reader);
            }
            connection.Close();
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
            reader = DBHelper.ExecQuery(query, connection);
            List<Card> card = new List<Card>();
            while (reader.Read())
            {
                card.Add(GetCardInfo(reader));
            }
            connection.Close();
            return card;
        }
        public Card GetCardByWord()
        {
            query = @"SELECT max(card_id) from Card where card_id like 'CM%' ;";
            reader = DBHelper.ExecQuery(query, connection);
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
            connection.Close();
            return card;
        }
    }
}