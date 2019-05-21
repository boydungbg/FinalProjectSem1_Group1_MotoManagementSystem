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
            Card_Logs cardLogs = new Card_Logs("CM06", "security_01", "75-G1-2222", new DateTime(2019, 05, 20), null, null, null);
            Assert.True(cardLogsDAL.CreateCardLogs(cardLogs));
            cardLogsDAL = new Card_LogsDAL();
            cardLogs = new Card_Logs(null, null, null, null, DateTime.Now, "08:00", 0);
            Assert.True(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "75-G1-2222", "CM06", "2019-05-20 00:00:00"));
            cardLogsDAL = new Card_LogsDAL();
            Assert.True(cardLogsDAL.DeleteCardLogsByID("CM06"));
        }
        [Fact]
        public void CreateCardLogsTest2()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = new Card_Logs();
            Assert.False(cardLogsDAL.CreateCardLogs(null));
            cardLogsDAL = new Card_LogsDAL();
            cardLogs = new Card_Logs(null, null, null, null, DateTime.Now, "07:00", 0);
            Assert.False(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(null, "CM06", "75-G1-2222", "2019-05-20 00:00:00"));
            cardLogsDAL = new Card_LogsDAL();
            Assert.False(cardLogsDAL.DeleteCardLogsByID(null));
        }
        [Fact]
        public void GetCardLogsByLinceseAndCardIDTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = cardLogsDAL.GetCardLogsByLicensePlateAndCardID("11-X8-2222", "CM01");
            Assert.Null(cardLogs);
        }
    }
}