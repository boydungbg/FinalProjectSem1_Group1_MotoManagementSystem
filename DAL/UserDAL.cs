using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class UserDAL
    {
        private MySqlConnection connection;
        private MySqlDataReader reader;
        private string query;
        public UserDAL()
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
        public User GetUserByUsernameAndPassWord(string username, string password)
        {
            query = @"select * from Accounts where acc_name = '" + username + "' and acc_pass = '" + password + "' ;";
            reader = DBHelper.ExecQuery(query, connection);
            User user = null;
            if (reader.Read())
            {
                user = GetUserInfo(reader);
            }
            connection.Close();
            return user;
        }
        private User GetUserInfo(MySqlDataReader reader)
        {
            User user = new User();
            user.User_name = reader.GetString("acc_name");
            user.User_pass = reader.GetString("acc_pass");
            user.User_level = reader.GetInt32("acc_level");
            return user;
        }
    }
}