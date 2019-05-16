using System;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CardDAL
    {
        private MySqlConnection connection;

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
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            //Lock table 
            command.CommandText = @"lock tables Customer write, Card write, Card_Detail write;";
            command.ExecuteNonQuery();
            //Transaction
            MySqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                //Insert card
                string cardid = card.Card_id;
                string cardType = card.Card_type;
                string LicensePlate = card.LicensePlate;
                query = @"insert into Card(card_id,card_type,license_plate) values" +
                        "('" + cardid + "','" + cardType + "','" + LicensePlate + "');";
                command.CommandText = query;
                command.ExecuteNonQuery();
                // Insert Customer
                string cusid = cus.Cus_id;
                string cusname = cus.Cus_name;
                string cusAddress = cus.Cus_address;
                string licensePlate = cus.Cus_licensePlate;
                query = @"insert into Customer(cus_id,cus_fullname,cus_address,license_plate) values" +
                        "('" + cusid + "', '" + cusname + "', '" + cusAddress + "','" + licensePlate + "');";
                command.CommandText = query;
                command.ExecuteNonQuery();
                // Insert Card_detail
                string card_id = card_Detail.Card_id;
                string cus_id = card_Detail.Cus_id;
                string card_time = card_Detail.Card_time;
                query = @"insert into Card_detail(card_id,cus_id,card_time) values" +
                  "('" + card_id + "','" + cus_id + "','" + card_time + "');";
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
    }
}