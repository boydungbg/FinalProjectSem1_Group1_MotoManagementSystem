using System;
using System.Collections.Generic;
using Persistence;
using Xunit;

namespace DAL.Xunit
{
    public class Card_LogsUnitTest
    {
        [Fact]
        public void CreateCardLogsAndUpdateTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = new Card_Logs("CM06", "security_01", "75G1-2222", new DateTime(2019, 05, 20), null, null, null);
            Assert.True(cardLogsDAL.CreateCardLogs(cardLogs));
            cardLogs = new Card_Logs(null, null, null, null, DateTime.Now, "08:00", 0);
            cardLogsDAL = new Card_LogsDAL();
            Assert.True(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "75G1-2222", "CM06", "2019-05-26 00:00:00"));
        }
        [Fact]
        public void GetCardLogsByCardIDAndLicensePlateTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = cardLogsDAL.GetCardLogsByCardIDAndLicensePlate("CM99", "75G1-4422");
            Assert.Null(cardLogs);
        }
        [Fact]
        public void GetListCardLogsTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2019-05-26", "2019-05-31");
            Assert.NotNull(listCardLogs);
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