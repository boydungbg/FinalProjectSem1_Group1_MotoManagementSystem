using System;
using Xunit;
using DAL;
using Persistence;

namespace DAL.test
{
    public class UserUnitTest
    {

        [Theory]
        [InlineData("manager_01", "24122000")]
        [InlineData("security_01", "24122000")]
        public void LoginTest1(string username, string password)
        {
            UserDAL userDAL = new UserDAL();
            User user = userDAL.Login(username, password);

            Assert.NotNull(user);
            // Assert.Equal(username, user.User_name);
        }
        [Theory]
        [InlineData("customer_01", "123456789")]
        [InlineData("'?^%'", "'.:=='")]
        [InlineData("'?^%'", null)]
        [InlineData(null, "'.:=='")]
        public void LoginTest2(string username, string password)
        {
            UserDAL userDAL = new UserDAL();
            Assert.Null(userDAL.Login(username, password));
        }
    }
}