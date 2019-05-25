using System;
using Xunit;
using DAL;
using Persistence;

namespace DAL.Xunit
{
    public class UserUnitTest
    {
        [Theory]
        [InlineData("manager_01", "24122000")]
        [InlineData("security_01", "24122000")]
        public void LoginTest1(string username, string password)
        {
            UserDAL userDAL = new UserDAL();
            User user = userDAL.GetUserByUsernameAndPassWord(username, password);
            Assert.NotNull(user);
            Assert.Equal(username, user.User_name);
        }
        
        [Theory]
        [InlineData("manager_02", "123456789")]
        [InlineData("manager_02", "1231123")]
        [InlineData("21312", null)]
        [InlineData(null, "4234123")]
        public void LoginTest2(string username, string password)
        {
            UserDAL userDAL = new UserDAL();
            Assert.Null(userDAL.GetUserByUsernameAndPassWord(username, password));
        }
    }
}