using System;
using System.Collections.Generic;
using Persistence;
using Xunit;
namespace DAL.Xunit
{
    public class CardUnitTest
    {
        [Fact]
        public void CreateCardTest1()
        {
            CardDAL cardDAL = new CardDAL();
            DateTime start_day = new DateTime(2019, 5, 17);
            DateTime end_day = new DateTime(2019, 6, 17);
            Card card = new Card("CM21", "89B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89B5-8888");
            Card_Detail card_Detail = new Card_Detail("CM21", "101029011", start_day, end_day);
            Assert.True(cardDAL.CreateCard(card, cus, card_Detail));
            cardDAL = new CardDAL();
            card = new Card(null, "89B5-9988", null, 1, null, null, null);
            Assert.True(cardDAL.UpdateCardByID(card, "CM21"));
            cardDAL = new CardDAL();
            Assert.True(cardDAL.DeleteCardByID("CM21", "101029011"));
        }
        [Fact]
        public void CreateCardTest2()
        {
            CardDAL cardDAL = new CardDAL();
            DateTime start_day = new DateTime(2019, 05, 17);
            DateTime end_day = new DateTime(2019, 06, 17);
            Card card = new Card("CM21", "89B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89B5-8888");
            Card_Detail card_Detail = new Card_Detail("CM21", "101029011", start_day, end_day);
            Assert.False(cardDAL.CreateCard(null, cus, card_Detail));
            cardDAL = new CardDAL();
            card = new Card(null, "89B5-9988", null, 1, null, null, null);
            Assert.False(cardDAL.UpdateCardByID(null, null));
            cardDAL = new CardDAL();
            Assert.False(cardDAL.DeleteCardByID(null, "100000000"));
        }
        [Theory]
        [InlineData("CM01")]
        [InlineData("CM02")]
        public void GetCardByIDTest1(string card_id)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByID(card_id);
            Assert.NotNull(card);
            Assert.Equal(card_id, card.Card_id);
        }
        [Theory]
        [InlineData("CM40")]
        [InlineData("CM41")]
        public void GetCardByIDTest2(string card_id)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByID(card_id);
            Assert.Null(card);
        }
        [Fact]
        public void ShowlistCardTest1()
        {
            CardDAL cardDAL = new CardDAL();
            List<Card> card = cardDAL.GetlistCard();
            Assert.NotEmpty(card);
        }
        [Theory]
        [InlineData("44S1-4422")]
        [InlineData("22E1-2222")]
        public void GetCardByLicensePlateTest1(string licensePlate)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByLicensePlate(licensePlate);
            Assert.NotNull(card);
            Assert.Equal(licensePlate, card.LicensePlate);
        }
        [Theory]
        [InlineData("89B5-9988")]
        [InlineData(null)]
        public void GetCardByLicensePlateTest2(string licensePlate)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByLicensePlate(licensePlate);
            Assert.Null(card);
        }
        [Fact]
        public void GetCardByWordTest1()
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByWord();
            Assert.NotNull(card);
        }
    }
}