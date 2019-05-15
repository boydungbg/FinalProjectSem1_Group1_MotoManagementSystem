using System;
using Xunit;
using BL;

namespace BL_XUnit
{
    public class UserUnitTest
    {
        [Fact]
        public void LoginTest1()
        {
            UserBL userBL = new UserBL();
            Assert.NotNull(userBL.Login("manager_01", "24122000"));
        }

        [Fact]
        public void LoginTest2()
        {
            UserBL userBL = new UserBL();
            Assert.NotNull(userBL.Login("security_01", "24122000"));
        }
        [Fact]
        public void LoginTest3()
        {
            UserBL userBL = new UserBL();
            Assert.Null(userBL.Login("customer_01", "123456789"));
        }

        [Fact]
        public void LoginTest4()
        {
            UserBL userBL = new UserBL();
            Assert.Null(userBL.Login("'#!@#!@'", "'><?<>'"));
        }
    }
}
