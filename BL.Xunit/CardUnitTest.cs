using System;
using System.Collections.Generic;
using Persistence;
using Xunit;

namespace BL.Xunit
{
    public class CardUnitTest
    {
        [Fact]
        public void CreateCardTest1()
        {
            CardBL cardBL = new CardBL();
            DateTime start_day = new DateTime(2019, 05, 17);
            DateTime end_day = new DateTime(2019, 06, 17);
            string cardType = "Thẻ tháng";
            Card_Detail card_Detail = new Card_Detail(10012, "101029011", start_day, end_day);
            Card card = new Card(10099, "89-B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89-B5-8888");
            Assert.True(cardBL.CreateCard(card, cus, card_Detail));
            cardBL = new CardBL();
            card = new Card(null, "89-B5-9988", cardType, 1, null, null, null);
            Assert.True(cardBL.UpdateCardByID(card, 10099));
            cardBL = new CardBL();
            Assert.True(cardBL.DeleteCardByID(10099, "101029011"));
        }
        [Fact]
        public void CreateCardTest2()
        {
            CardBL cardBL = new CardBL();
            DateTime start_day = new DateTime(2019, 05, 17);
            DateTime end_day = new DateTime(2019, 06, 17);
            string cardType = "Thẻ tháng";
            Card_Detail card_Detail = new Card_Detail(10012, "101029011", start_day, end_day);
            Card card = new Card(10099, "89-B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89-B5-8888");
            Assert.False(cardBL.CreateCard(null, cus, card_Detail));
            cardBL = new CardBL();
            card = new Card(10099, "89-B5-9988", cardType, 1, null, null, null);
            Assert.False(cardBL.UpdateCardByID(null, 10099));
            cardBL = new CardBL();
            Assert.False(cardBL.DeleteCardByID(0, "101029011"));
        }
        [Theory]
        [InlineData(10011)]
        [InlineData(10012)]
        public void GetCardByIDTest1(int cardid)
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByID(cardid);
            Assert.NotNull(card);
            Assert.Equal(cardid, card.Card_id);
        }
        [Theory]
        [InlineData(10099)]
        [InlineData(123123)]
        public void GetCardByIDTest2(int cardid)
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByID(cardid);
            Assert.Null(card);
        }
        [Fact]
        public void GetlistCardTest1()
        {
            CardBL cardBL = new CardBL();
            List<Card> card = cardBL.GetlistCard();
            Assert.NotEmpty(card);
        }
        [Theory]
        [InlineData("88A1-8888")]
        [InlineData("44S1-4422")]
        public void GetCardByLicensePlateTest1(string licensePlate)
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByLicensePlate(licensePlate);
            Assert.NotNull(card);
            Assert.Equal(licensePlate, card.LicensePlate);
        }
        [Theory]
        [InlineData("44B1-5544")]
        [InlineData("//$@#$@")]
        [InlineData(null)]
        public void GetCardByLicensePlateTest2(string licensePlate)
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByLicensePlate(licensePlate);
            Assert.Null(card);
        }
        [Fact]
        public void GetCardIDTest1()
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardID();
            Assert.NotNull(card);
        }
    }
}