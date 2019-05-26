using System;
using System.Collections.Generic;
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
            Card_Logs cardLogs = new Card_Logs("CM06", "security_01", "75G1-2222", new DateTime(2019, 05, 20), null, null, null);
            Assert.True(cardLogsDAL.CreateCardLogs(cardLogs));
            cardLogsDAL = new Card_LogsDAL();
            cardLogs = new Card_Logs(null, null, null, null, DateTime.Now, "08:00", 0);
            Assert.True(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "75G1-2222", "CM06", "2019-05-26 00:00:00"));
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
            Assert.False(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(null, "CM06", "75G1-2222", "2019-05-20 00:00:00"));
            cardLogsDAL = new Card_LogsDAL();
            Assert.False(cardLogsDAL.DeleteCardLogsByID(null));
        }
        [Fact]
        public void GetCardLogsByCardIDTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = cardLogsDAL.GetCardLogsByCardID("CM99");
            Assert.Null(cardLogs);
        }
        [Fact]
        public void GetListCardLogsTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2019-05-26", "2019-05-27");
            Assert.NotEmpty(listCardLogs);
        }
        [Fact]
        public void GetListCardLogsTest2()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2000-05-26", "2000-05-27");
            Assert.Empty(listCardLogs);
        }
    }
}