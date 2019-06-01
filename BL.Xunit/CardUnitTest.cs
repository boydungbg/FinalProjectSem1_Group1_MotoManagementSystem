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
            Card_Detail card_Detail = new Card_Detail("CM21", "101029011", start_day, end_day);
            Card card = new Card("CM21", "89-B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89-B5-8888");
            Assert.True(cardBL.CreateCard(card, cus, card_Detail));
            cardBL = new CardBL();
            card = new Card("CM21", "89-B5-9988", cardType, 1, null, null, null);
            Assert.True(cardBL.UpdateCardByID(card, "CM21"));
            cardBL = new CardBL();
            Assert.True(cardBL.DeleteCardByID("CM21", "101029011"));
        }
        [Fact]
        public void CreateCardTest2()
        {
            CardBL cardBL = new CardBL();
            DateTime start_day = new DateTime(2019, 05, 17);
            DateTime end_day = new DateTime(2019, 06, 17);
            string cardType = "Thẻ tháng";
            Card_Detail card_Detail = new Card_Detail("CM21", "101029011", start_day, end_day);
            Card card = new Card("CM21", "89-B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89-B5-8888");
            Assert.False(cardBL.CreateCard(null, cus, card_Detail));
            cardBL = new CardBL();
            card = new Card("CM21", "89-B5-9988", cardType, 1, null, null, null);
            Assert.False(cardBL.UpdateCardByID(null, "CM21"));
            cardBL = new CardBL();
            Assert.False(cardBL.DeleteCardByID(null, "101029011"));
        }
        [Theory]
        [InlineData("CM01")]
        [InlineData("CM02")]
        public void GetCardByIDTest1(string cardid)
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByID(cardid);
            Assert.NotNull(card);
            Assert.Equal(cardid, card.Card_id);
        }
        [Theory]
        [InlineData("CM30")]
        [InlineData(null)]
        public void GetCardByIDTest2(string cardid)
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
        public void GetCardByWordTest1()
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByWord();
            Assert.NotNull(card);
        }
    }
}