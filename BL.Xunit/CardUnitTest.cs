using System;
using System.Collections.Generic;
using Persistence;
using Xunit;

namespace BL.Xunit
{
    public class CardUnitTest
    {
        [Fact]
        public void CreateCardAndUpdateTest1()
        {
            DateTime start_day = new DateTime(2019, 5, 17);
            DateTime end_day = new DateTime(2019, 6, 17);
            Card card = new Card(10099, "101029011", "89B5-9988", "Thẻ tháng", 1, start_day, end_day, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89B5-8888");
            CardBL cardBL = new CardBL();
            Assert.True(cardBL.CreateCard(card, cus));
            cardBL = new CardBL();
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
            Card card = new Card(10099, "101029011", "89B5-9988", "Thẻ tháng", null, start_day, end_day, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89-B5-8888");
            Assert.False(cardBL.CreateCard(null, cus));
            cardBL = new CardBL();
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
        [InlineData(0)]
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
            List<Card> card = cardBL.GetlistCard(0);
            Assert.NotEmpty(card);
        }
        [Fact]
        public void GetlistCardMonthTest1()
        {
            CardBL cardBL = new CardBL();
            List<Card> card = cardBL.GetlistCardMonth(0);
            Assert.NotEmpty(card);
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