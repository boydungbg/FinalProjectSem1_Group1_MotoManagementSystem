using System;
using System.Collections.Generic;
using Persistence;
using Xunit;

namespace BL.Xunit
{
    public class Card_LogsUnitTest
    {
        [Fact]
        public void CreateCardLogsTest1()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = new Card_Logs("CM06", "security_01", "75G1-2222", new DateTime(2019, 05, 20), null, null, null);
            Assert.True(cardLogsBL.CreateCardLogs(card_Logs));
            cardLogsBL = new Card_LogsBL();
            card_Logs = new Card_Logs(null, null, null, null, DateTime.Now, "07:00", 0);
            Assert.True(cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(card_Logs, "CM06", "75G1-2222", "2019-05-20 00:00:00"));
        }
        [Fact]
        public void CreateCardLogsTest2()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = new Card_Logs("CM06", "security_01", "75G1-2222", new DateTime(2019, 05, 20), null, null, null);
            Assert.False(cardLogsBL.CreateCardLogs(null));
            cardLogsBL = new Card_LogsBL();
            card_Logs = new Card_Logs(null, null, null, null, DateTime.Now, "07:00", 0);
            Assert.False(cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(null, "CM06", "75G1-2222", "2019-05-20 00:00:00"));
        }
        [Fact]
        public void GetCardLogsByLinceseAndCardIDTest1()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = cardLogsBL.GetCardLogsByCardIDAndLicensePlate("CM99", "75G1-4422");
            Assert.Null(card_Logs);
        }
        [Fact]
        public void GetListCardLogsTest1()
        {
            Card_LogsBL cardLogsDAL = new Card_LogsBL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2019-05-26", "2019-05-27");
            Assert.NotNull(listCardLogs);
        }
        [Fact]
        public void GetListCardLogsTest2()
        {
            Card_LogsBL cardLogsDAL = new Card_LogsBL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2000-05-26", "2000-05-27");
            Assert.Empty(listCardLogs);
        }
    }
}