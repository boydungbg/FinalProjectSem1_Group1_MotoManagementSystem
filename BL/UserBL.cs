using System;
using Persistence;
using DAL;
using System.Text.RegularExpressions;

namespace BL
{
    public class UserBL
    {

        private UserDAL userdal;
        public UserBL()
        {
            userdal = new UserDAL();
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
            return userdal.GetUserByUsernameAndPassWord(username, password);
        }
    }
}

