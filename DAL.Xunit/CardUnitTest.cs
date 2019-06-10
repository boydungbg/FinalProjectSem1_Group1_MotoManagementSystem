using System;
using System.Collections.Generic;
using Persistence;
using Xunit;
namespace DAL.Xunit
{
    public class CardUnitTest
    {
        [Fact]
        public void CreateCardAndUpdateTest1()
        {
            DateTime start_day = new DateTime(2019, 5, 17);
            DateTime end_day = new DateTime(2019, 6, 17);
            Card card = new Card(10099, "123123456", "89B5-9988", "Thẻ tháng", 1, start_day, end_day, null, null);
            Customer cus = new Customer("123123456", "Lê Chí Dũng", "Bắc Giang", "89B5-8888");
            CardDAL cardDAL = new CardDAL();
            Assert.True(cardDAL.CreateCard(card, cus));
            cardDAL = new CardDAL();
            Assert.True(cardDAL.UpdateCardByID(card, 10099));
            cardDAL = new CardDAL();
            Assert.True(cardDAL.DeleteCardByID(10099, "123123456"));
        }
        [Theory]
        [InlineData(10011)]
        [InlineData(10012)]
        public void GetCardByIDTest1(int card_id)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByID(card_id);
            Assert.NotNull(card);
            Assert.Equal(card_id, card.Card_id);
        }
        [Theory]
        [InlineData(10099)]
        [InlineData(12231)]
        public void GetCardByIDTest2(int card_id)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByID(card_id);
            Assert.Null(card);
        }
        [Fact]
        public void GetlistCardTest1()
        {
            CardDAL cardDAL = new CardDAL();
            List<Card> card = cardDAL.GetlistCard(0);
            Assert.NotEmpty(card);
        }
        [Fact]
        public void GetlistCardMonthTest1()
        {
            CardDAL cardDAL = new CardDAL();
            List<Card> card = cardDAL.GetlistCardMonth(0);
            Assert.NotEmpty(card);
        }
        [Fact]
        public void GetCardIDTest1()
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardID();
            Assert.NotNull(card);
        }
    }
}