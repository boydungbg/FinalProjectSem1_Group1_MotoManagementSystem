using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CustomerDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public CustomerDAL()
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
        public Customer GetCustomerByID(string customerid)
        {
            query = @"select * from Customer where cus_id = '" + customerid + "' ;";
            reader = DBHelper.ExecQuery(query, connection);
            Customer cus = null;
            if (reader.Read())
            {
                cus = GetCustomerInfo(reader);
            }
            connection.Close();
            return cus;
        }
        public Customer GetCustomerByLincese_plate(string Lincese_plate)
        {
            query = @"select * from Customer where license_plate = '" + Lincese_plate + "' ;";
            reader = DBHelper.ExecQuery(query, connection);
            Customer cus = null;
            if (reader.Read())
            {
                cus = GetCustomerInfo(reader);
            }
            connection.Close();
            return cus;

        }
        private Customer GetCustomerInfo(MySqlDataReader reader)
        {
            Customer cus = new Customer();
            if (reader.IsDBNull(reader.GetOrdinal("cus_id")))
            {
                return null;
            }
            cus.Cus_id = reader.GetString("cus_id");
            cus.Cus_name = reader.GetString("cus_fullname");
            cus.Cus_address = reader.GetString("cus_address");
            cus.Cus_licensePlate = reader.GetString("license_plate");
            return cus;
        }
    }
}