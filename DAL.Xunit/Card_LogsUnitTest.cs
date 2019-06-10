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
            Card_Logs cardLogs = new Card_Logs(10011, "security_01", "33G1-3333", new DateTime(2019, 05, 20), null, null, null);
            Assert.True(cardLogsDAL.CreateCardLogs(cardLogs));
            cardLogs = new Card_Logs(10011, null, null, null, DateTime.Now, 1, 10000);
            cardLogsDAL = new Card_LogsDAL();
            Assert.True(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "75G1-2222", 10011, "2019-05-26 00:00:00"));
        }
        [Fact]
        public void CreateCardLogsAndUpdateTest2()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = new Card_Logs(10015, "security_01", "88A1-8888", new DateTime(2019, 05, 20), null, null, 10000);
            Assert.True(cardLogsDAL.CreateCardLogs(cardLogs));
            cardLogs = new Card_Logs(10015, null, null, null, DateTime.Now, 1, 10000);
            cardLogsDAL = new Card_LogsDAL();
            Assert.True(cardLogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, "88A1-8888", 10015, "2019-05-26 00:00:00"));
        }
        [Fact]
        public void GetCardLogsByCardIDAndLicensePlateTest1()
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs cardLogs = cardLogsDAL.GetCardLogsByCardIDAndLicensePlate(10099, "75G1-4422");
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
        [Theory]
        [InlineData(10099, 0)]
        [InlineData(0, 0)]
        public void GetCardByIDTest2(int cardID, int status)
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs card_logs = cardLogsDAL.GetCardLogsByID(cardID, status);
            Assert.Null(card_logs);
        }
        [Theory]
        [InlineData("72222")]
        [InlineData("88888")]
        public void GetCardLogsByLicensePlateTest2(string licensePlate)
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            Card_Logs card_logs = cardLogsDAL.GetCardLogsByLicensePlate(licensePlate);
            Assert.Null(card_logs);
        }
        [Theory]
        [InlineData(0, "2019-06-10", "2019-06-20", "Thẻ ngày")]
        [InlineData(0, "2019-06-10", "2019-06-20", "Thẻ tháng")]
        public void GetListCardLogsByPageTest1(int page, string from, string to, string type)
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogsByPage(page, from, to, type);
            Assert.NotEmpty(listCardLogs);
        }
        [Theory]
        [InlineData(0, "2000-06-10", "2000-06-20", "Thẻ xxx")]
        [InlineData(0, "2000-06-10", "2000-06-20", "Thẻ xxxx")]
        public void GetListCardLogsByPageTest2(int page, string from, string to, string type)
        {
            Card_LogsDAL cardLogsDAL = new Card_LogsDAL();
            List<Card_Logs> listCardLogs = cardLogsDAL.GetListCardLogsByPage(page, from, to, type);
            Assert.Empty(listCardLogs);
        }
    }
}