using System;
using Persistence;
using Xunit;

namespace DAL.Xunit
{
    public class Card_LogsUnitTest
    {
        [Fact]
        public void CreateCardLogsTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = new Card_Logs("CM01", "security_01", "123456789", DateTime.Now, null, null, null);
            Assert.True(cardLogsDAL.CreateCardLogs(cardLogs));
            cardLogsDAL = new Card_LogsDAL();
            Assert.True(cardLogsDAL.DeleteCardLogsByID("CM01"));
        }
        [Fact]
        public void CreateCardLogsTest2()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = new Card_Logs();
            Assert.False(cardLogsDAL.CreateCardLogs(null));
            cardLogsDAL = new Card_LogsDAL();
            Assert.False(cardLogsDAL.DeleteCardLogsByID(null));
        }
        [Fact]
        public void GetCardLogsByLinceseTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = cardLogsDAL.GetCardLogsByLicensePlate("11-X8-2222");
            Assert.Null(cardLogs);
        }
    }
}