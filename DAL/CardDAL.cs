using System;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CardDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public CardDAL()
        {
            connection = DBHelper.OpenConnection();
        }
        public bool CreateCard(Card card, Customer cus, Card_Detail card_Detail)
        {
            bool result = false;
            if (card == null || cus == null || card_Detail == null)
            {
                return result;
            }
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
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
                query = @"insert into Card(card_id,card_type,license_plate) 
                values  (@cardid,@cardType,@LicensePlate);";
                command.Parameters.AddWithValue("@cardid", card.Card_id);
                command.Parameters.AddWithValue("@cardType", card.Card_type);
                command.Parameters.AddWithValue("@LicensePlate", card.LicensePlate);
                command.CommandText = query;
                command.ExecuteNonQuery();
                // Insert Customer
                query = @"insert into Customer(cus_id,cus_fullname,cus_address,license_plate)
                values   (@cus_id,@cus_name,@cus_address,@license_Plate);";
                command.Parameters.AddWithValue("@cus_id", cus.Cus_id);
                command.Parameters.AddWithValue("@cus_name", cus.Cus_name);
                command.Parameters.AddWithValue("@cus_address", cus.Cus_address);
                command.Parameters.AddWithValue("@license_Plate", cus.Cus_licensePlate);
                command.CommandText = query;
                command.ExecuteNonQuery();
                // Insert Card_detail
                query = @"insert into Card_detail (card_id,cus_id,start_day,end_day)
                        values (@card_id_card_detail,@cus_id_card_detail,@start_day,@end_day);";
                command.Parameters.AddWithValue("@card_id_card_detail", card_Detail.Card_id);
                command.Parameters.AddWithValue("@cus_id_card_detail", card_Detail.Cus_id);
                command.Parameters.AddWithValue("@start_day", card_Detail.Start_day);
                command.Parameters.AddWithValue("@end_day", card_Detail.End_day);
                command.CommandText = query;
                command.ExecuteNonQuery();
                transaction.Commit();
                result = true;
            }
            catch (System.Exception e)
            {
                string m = e.Message;
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
        public bool DeleteCardByID(string cardid, string cusid)
        {
            bool result = false;
            if (cardid == null || cusid == null)
            {
                return result;
            }
            if (connection == null)
            {
                connection = DBHelper.OpenConnection();
            }
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            MySqlCommand command = new MySqlCommand("", connection);
            // //Lock table 
            // command.CommandText = @"lock tables Customer read, Card read, Card_Detail read;";
            // command.ExecuteNonQuery();
            // //Transaction
            // MySqlTransaction transaction = connection.BeginTransaction();
            // command.Transaction = transaction;
            // try
            // {
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
            // transaction.Commit();
            result = true;
            // }
            // catch (System.Exception e)
            // {
            //     string m = e.Message;
            //     transaction.Rollback();
            // }
            // finally
            // {
            //     // Unlock Tables
            //     command.CommandText = "unlock tables";
            //     command.ExecuteNonQuery();
            //     connection.Close();
            // }
            return result;
        }
        public Card GetCardByID(string cardid)
        {
            if (cardid == null)
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
            MySqlCommand command = new MySqlCommand("", connection);
            query = @"select * from Card where card_id = @cardid ;";
            command.Parameters.AddWithValue("@cardid", cardid);
            command.CommandText = query;
            Card card = null;
            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    card = GetCard(reader);
                }
            }
            return card;
        }
        public Card GetCard(MySqlDataReader reader)
        {
            string cardid = reader.GetString("card_id");
            string licensePlate = reader.GetString("license_plate");
            string cardType = reader.GetString("card_type");
            int cardstatus = reader.GetInt32("card_status");
            Card card = new Card(cardid, licensePlate, cardType, cardstatus, null, null);
            return card;
        }
    }
}