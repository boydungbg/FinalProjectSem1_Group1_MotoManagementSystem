using System;
using System.Collections.Generic;
using Persistence;
using Xunit;

namespace BL.Xunit
{
    public class Card_LogsUnitTest
    {
        [Fact]
        public void CreateCardLogsAndUpdateTest1()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs cardLogs = new Card_Logs(10011, "security_01", "33G1-3333", new DateTime(2019, 06, 05), null, null, null);
            Assert.True(cardLogsBL.CreateCardLogs(cardLogs));
            cardLogs = new Card_Logs(10011, null, null, null, DateTime.Now, 10000, 1);
            cardLogsBL = new Card_LogsBL();
            Assert.True(cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "33G1-3333", 10011, "2019-06-05 00:00:00"));
        }
        [Fact]
        public void CreateCardLogsAndUpdateTest2()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs cardLogs = new Card_Logs(10015, "security_01", "88A1-8888", new DateTime(2019, 06, 05), null, null, null);
            Assert.True(cardLogsBL.CreateCardLogs(cardLogs));
            cardLogs = new Card_Logs(10015, null, null, null, DateTime.Now, 10000, 1);
            cardLogsBL = new Card_LogsBL();
            Assert.True(cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "88A1-8888", 10015, "2019-06-05 00:00:00"));
        }
        [Fact]
        public void CreateCardLogsAndUpdateTest3()
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = new Card_Logs(10012, "security_01", "75G1-2222", new DateTime(2019, 06, 05), null, null, null);
            Assert.False(cardLogsBL.CreateCardLogs(null));
            cardLogsBL = new Card_LogsBL();
            card_Logs = new Card_Logs(10012, null, null, null, DateTime.Now, 10000, 1);
            Assert.False(cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(null, "75G1-2222", 10012, "2019-05-20 00:00:00"));
        }
        [Theory]
        [InlineData(10011, "33G1-3333")]
        [InlineData(10015, "88A1-8888")]
        public void GetCardLogsByLinceseAndCardIDTest1(int cardID, string licensePlate)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = cardLogsBL.GetCardLogsByCardIDAndLicensePlate(cardID, licensePlate);
            Assert.NotNull(card_Logs);
        }
        [Theory]
        [InlineData(123123, "@#!@#")]
        [InlineData(123123, null)]
        public void GetCardLogsByLinceseAndCardIDTest2(int cardID, string licensePlate)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_Logs = cardLogsBL.GetCardLogsByCardIDAndLicensePlate(cardID, licensePlate);
            Assert.Null(card_Logs);
        }
        [Fact]
        public void GetListCardLogsTest1()
        {
            Card_LogsBL cardLogsDAL = new Card_LogsBL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2019-05-26", "2019-06-27", "");
            Assert.NotNull(listCardLogs);
        }
        [Fact]
        public void GetListCardLogsTest2()
        {
            Card_LogsBL cardLogsDAL = new Card_LogsBL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogs("2000-05-26", "2000-06-27", "");
            Assert.Empty(listCardLogs);
        }
        [Theory]
        [InlineData(10011, 1)]
        [InlineData(10015, 1)]
        public void GetListCardByIDAndStatusTest1(int cardID, int status)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_logs = cardLogsBL.GetCardLogsByID(cardID, status);
            Assert.NotNull(card_logs);
        }
        [Theory]
        [InlineData(10099, 1)]
        [InlineData(0, 0)]
        public void GetListCardByIDAndStatusTest2(int cardID, int status)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_logs = cardLogsBL.GetCardLogsByID(cardID, status);
            Assert.Null(card_logs);
        }
        [Theory]
        [InlineData("33G1-3333")]
        [InlineData("88A1-8888")]
        public void GetCardLogsByLicensePlateTest1(string licensePlate)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_logs = cardLogsBL.GetCardLogsByLicensePlate(licensePlate);
            Assert.NotNull(card_logs);
        }
        [Theory]
        [InlineData("72222")]
        [InlineData("88888")]
        public void GetCardLogsByLicensePlateTest2(string licensePlate)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            Card_Logs card_logs = cardLogsBL.GetCardLogsByLicensePlate(licensePlate);
            Assert.Null(card_logs);
        }
        [Theory]
        [InlineData(0, "2019-06-01", "2019-06-20", "Thẻ ngày")]
        [InlineData(0, "2019-06-01", "2019-06-20", "Thẻ tháng")]
        public void GetListCardLogsByPageTest1(int page, string from, string to, string type)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            List<Card_Logs> listCardLogs = cardLogsBL.GetListCardLogsByPage(page, from, to, type);
            Assert.NotNull(listCardLogs);
        }
        [Theory]
        [InlineData(-1, "2000-06-10", "2000-06-20", "Thẻ ngày")]
        [InlineData(0, "!@#!@#", "2019-06-20", null)]
        public void GetListCardLogsByPageTest2(int page, string from, string to, string type)
        {
            Card_LogsBL cardLogsBL = new Card_LogsBL();
            List<Card_Logs> listCardLogs = cardLogsBL.GetListCardLogsByPage(page, from, to, type);
            Assert.Null(listCardLogs);
        }
    }
}