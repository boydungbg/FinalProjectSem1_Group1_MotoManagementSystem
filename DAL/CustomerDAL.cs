using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CustomerDAL
    {
        private MySqlDataReader reader;
        private string query;
        public Customer GetCustomerByID(string customerid)
        {
            if (customerid == null)
            {
                return null;
            }
            query = @"select * from Customer where cus_id = '" + customerid + "' ;";
            DBHelper.OpenConnection();
            // reader = DBHelper.ExecQuery(query);
            Customer cus = null;
            reader = DBHelper.ExecQuery(query);
            if (reader.Read())
            {
                cus = GetCustomerInfo(reader);
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
            return cus;
        }
        public Customer GetCustomerByLincese_plate(string Lincese_plate)
        {
            if (Lincese_plate == null)
            {
                return null;
            }
            query = @"select * from Customer where license_plate = '" + Lincese_plate + "' ;";
            DBHelper.OpenConnection();
            // reader = DBHelper.ExecQuery(query);
            Customer cus = null;
            reader = DBHelper.ExecQuery(query);
            if (reader.Read())
            {
                cus = GetCustomerInfo(reader);
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
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