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
            connection = DBHelper.OpenConnection();
        }
        public User Login(string username, string password)
        {
            if ((username == null) || (password == null))
            {
                return null;
            }
            Regex regex = new Regex("[a-zA-Z0-9_]");
            MatchCollection matchCollectionUsername = regex.Matches(username);
            MatchCollection matchCollectionPassword = regex.Matches(password);
            if (matchCollectionUsername.Count < username.Length || matchCollectionPassword.Count < password.Length)
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
            query = @"select * from Accounts where acc_name = '" + username + "' and acc_pass = '" + password + "';";

            MySqlCommand command = new MySqlCommand(query, connection);
            User user = null;
            using (reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = GetUser(reader);
                }
            }
            return user;
        }
        private User GetUser(MySqlDataReader reader)
        {
            if (reader.GetString("acc_id")==null)
            {
                return null;
            }
            string userid = reader.GetString("acc_id");
            string username = reader.GetString("acc_name");
            string userpass = reader.GetString("acc_pass");
            string fullname = reader.GetString("acc_fullname");
            string email = reader.GetString("acc_email");
            int level = reader.GetInt16("acc_level");
            DateTime acc_dateCreated = reader.GetDateTime("acc_dateCreated");
            User user = new User(userid, username, userpass, fullname, email, level, acc_dateCreated);

            return user;
        }


    }
}