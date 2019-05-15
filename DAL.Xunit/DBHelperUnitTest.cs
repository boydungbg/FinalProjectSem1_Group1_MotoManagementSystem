using System;
using Xunit;
using DAL;


namespace CTS_DAL_XUnit
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
