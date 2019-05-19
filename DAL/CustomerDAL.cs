using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CustomerDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public Customer GetCustomerByID(string customerid)
        {
            if (customerid == null)
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
            query = @"select * from Customer where cus_id = @customerid ;";
            command.Parameters.AddWithValue("@customerid", customerid);
            command.CommandText = query;
            Customer cus = null;
            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cus = GetCustomer(reader);
                }
            }
            return cus;
        }
        public Customer GetCustomerByLincese_plate(string Lincese_plate)
        {
            if (Lincese_plate == null)
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
            query = @"select * from Customer where license_plate = @license_plate ;";
            command.Parameters.AddWithValue("@license_plate", Lincese_plate);
            command.CommandText = query;
            Customer cus = null;
            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cus = GetCustomer(reader);
                }
            }
            return cus;
        }
        public Customer GetCustomer(MySqlDataReader reader)
        {
            string customerid = reader.GetString("cus_id");
            string customerName = reader.GetString("cus_fullname");
            string customerAddress = reader.GetString("cus_address");
            string licensePlate = reader.GetString("license_plate");
            Customer cus = new Customer(customerid, customerName, customerAddress, licensePlate);
            return cus;
        }
    }
}