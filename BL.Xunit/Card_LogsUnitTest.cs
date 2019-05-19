using System;
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
            Card_Logs card_Logs = new Card_Logs("CM01", "security_01", "123456789", DateTime.Now, null, null, null);
            Assert.True(cardLogsBL.CreateCardLogs(card_Logs));
            cardLogsBL = new Card_LogsBL();
            Assert.True(cardLogsBL.DeleteCardLogsByID("CM01"));
        }
        [Fact]
        public void CreateCardLogsTest2()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = new Card_Logs();
            Assert.False(cardLogsBL.CreateCardLogs(null));
            cardLogsBL = new Card_LogsBL();
            Assert.False(cardLogsBL.DeleteCardLogsByID(null));
        }
        [Fact]
        public void GetCardLogsByLisencePlateTest1()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = cardLogsBL.GetCardLogsByLisencePlate("11-X9-9912");
            Assert.Null(card_Logs);
        }
    }
}