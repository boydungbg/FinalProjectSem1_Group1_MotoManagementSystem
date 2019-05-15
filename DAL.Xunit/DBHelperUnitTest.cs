using System;
using Xunit;
using DAL;


namespace DAL.Xunit
{
    public class DBHelperUnitTest
    {
        [Fact]
        public void OpenConnectionTest()
        {
            Assert.NotNull(DBHelper.OpenConnection());
        }
    }
}
