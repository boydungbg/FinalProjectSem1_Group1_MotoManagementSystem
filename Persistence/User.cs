using System;

namespace Persistence
{
    public class User
    {
        public string User_name { get; set; }
        public string User_pass { get; set; }
        public string User_fullname { get; set; }
        public string User_email { get; set; }
        public int User_level { get; set; }
        public DateTime User_dateCteated { get; set; }

        public User() { }
        public User(string user_name, string user_pass, string user_fullname, string user_email, int user_level, DateTime user_dateCteated)
        {
            this.User_name = user_name;
            this.User_pass = user_pass;
            this.User_fullname = user_fullname;
            this.User_email = user_email;
            this.User_level = user_level;
            this.User_dateCteated = user_dateCteated;
        }
    }
}
