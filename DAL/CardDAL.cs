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
        public bool CreateCard(Card card, Customer cus)
        {
            bool result = false;
            MySqlCommand command = new MySqlCommand("", connection);
            //Lock table 
            command.CommandText = @"lock tables Customer write, Card write;";
            command.ExecuteNonQuery();
            //Transaction
            MySqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                // Insert Customer
                command.CommandText = @"insert into Customer(cus_id,cus_fullname,cus_address,license_plate)
                values   (@cus_id,@cus_name,@cus_address,@license_Plate);";
                command.Parameters.AddWithValue("@cus_id", cus.Cus_id);
                command.Parameters.AddWithValue("@cus_name", cus.Cus_name);
                command.Parameters.AddWithValue("@cus_address", cus.Cus_address);
                command.Parameters.AddWithValue("@license_Plate", cus.Cus_licensePlate);
                command.ExecuteNonQuery();
                //Insert card
                command.CommandText = @"insert into Card(card_id,cus_id,card_type,license_plate,start_day,end_day) 
                values  (@card_id,@cus_id_card,@cardType,@LicensePlate,@start_day,@end_day);";
                command.Parameters.AddWithValue("@card_id", card.Card_id);
                command.Parameters.AddWithValue("@cus_id_card", cus.Cus_id);
                command.Parameters.AddWithValue("@LicensePlate", cus.Cus_licensePlate);
                command.Parameters.AddWithValue("@start_day", card.Start_day);
                command.Parameters.AddWithValue("@end_day", card.End_day);
                command.Parameters.AddWithValue("@cardType", card.Card_type);
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
        public bool UpdateCardByID(Card card, int cardid)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Update Card SET license_plate = @LicensePlate, card_status = @card_status, card_type =@card_type where  card_id = @card_id;";
            command.Parameters.AddWithValue("@LicensePlate", card.LicensePlate);
            command.Parameters.AddWithValue("@card_status", card.Card_Status);
            command.Parameters.AddWithValue("@card_type", card.Card_type);
            command.Parameters.AddWithValue("@card_id", cardid);
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public bool DeleteCardByID(int cardid, string cusid)
        {
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"Delete from Card where card_id = @cardid;";
            command.Parameters.AddWithValue("@cardid", cardid);
            command.CommandText = query;
            command.ExecuteNonQuery();
            query = @"Delete from Customer where cus_id = @cusid;";
            command.Parameters.AddWithValue("@cusid", cusid);
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public Card GetCardByID(int cardid)
        {
            query = @"select * from Card where card_id = " + cardid + " ;";
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
            if (reader.IsDBNull(reader.GetOrdinal("card_id")) || reader.IsDBNull(reader.GetOrdinal("cus_id")) || reader.IsDBNull(reader.GetOrdinal("start_day")) || reader.IsDBNull(reader.GetOrdinal("end_day")))
            {
                card.Cus_id = "Không có";
                card.Start_day = new DateTime();
                card.End_day = new DateTime();
            }
            else
            {
                card.Cus_id = reader.GetString("cus_id");
                card.Start_day = reader.GetDateTime("start_day");
                card.End_day = reader.GetDateTime("end_day");
            }
            card.Card_id = reader.GetInt32("card_id");
            card.LicensePlate = reader.GetString("license_plate");
            card.Card_type = reader.GetString("card_type");
            card.Card_Status = reader.GetInt32("card_status");
            card.Date_created = reader.GetDateTime("date_created");
            return card;
        }
        public List<Card> GetlistCard(int page)
        {
            query = @"select * from Card limit " + page + ",10;";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card> card = new List<Card>();
            while (reader.Read())
            {
                card.Add(GetCardInfo(reader));
            }
            connection.Close();
            return card;
        }
        public double GetCardNO()
        {
            query = @"select count(card_id) from Card;";
            reader = DBHelper.ExecQuery(query, connection);
            double CardNo = 0;
            while (reader.Read())
            {
                CardNo = reader.GetInt32("count(card_id)");
            }
            connection.Close();
            return CardNo;
        }
        public List<Card> GetlistCardMonth(int page)
        {
            query = @"select * from Card where card_type = 'Thẻ tháng' limit " + page + ",10;";
            reader = DBHelper.ExecQuery(query, connection);
            List<Card> card = new List<Card>();
            while (reader.Read())
            {
                card.Add(GetCardInfo(reader));
            }
            connection.Close();
            return card;
        }
        public double GetCardMonthNO()
        {
            query = @"select count(card_id) from Card where card_type = 'Thẻ tháng';";
            reader = DBHelper.ExecQuery(query, connection);
            double CardNo = 0;
            while (reader.Read())
            {
                CardNo = reader.GetInt32("count(card_id)");
            }
            connection.Close();
            return CardNo;
        }
        public Card GetCardID()
        {
            query = @"SELECT max(card_id) from Card ;";
            reader = DBHelper.ExecQuery(query, connection);
            Card card = null;
            if (reader.Read())
            {
                if (reader.IsDBNull(reader.GetOrdinal("max(card_id)")))
                {
                    return null;
                }
                card = new Card();
                card.Card_id = reader.GetInt32("max(card_id)");
            }
            connection.Close();
            return card;
        }
    }
}