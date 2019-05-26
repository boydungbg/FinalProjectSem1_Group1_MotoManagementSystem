using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class UserDAL
    {
        private MySqlDataReader reader;
        private string query;
        public User GetUserByUsernameAndPassWord(string username, string password)
        {
            if ((username == null) || (password == null))
            {
                return null;
            }
            query = @"select * from Accounts where acc_name = '" + username + "' and acc_pass = '" + password + "' ;";
            DBHelper.OpenConnection();
            reader = DBHelper.ExecQuery(query);
            User user = null;
            if (reader.Read())
            {
                user = GetUserInfo(reader);
            }
            // reader.Close();
            // reader.Dispose();
            DBHelper.CloseConnection();
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