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
            Card card = new Card(10099, "89B5-8888", "Thẻ tháng", null, null, null, null);
            Customer cus = new Customer("101029011", "Lê Chí Dũng", "Bắc Giang", "89B5-8888");
            Card_Detail card_Detail = new Card_Detail(10099, "101029011", start_day, end_day);
            CardDAL cardDAL = new CardDAL();
            Assert.True(cardDAL.CreateCard(card, cus, card_Detail));
            card = new Card(null, "89B5-9988", null, 1, null, null, null);
            cardDAL = new CardDAL();
            Assert.True(cardDAL.UpdateCardByID(card, 10099));
            cardDAL = new CardDAL();
            Assert.True(cardDAL.DeleteCardByID(10099, "101029011"));
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
        [InlineData("#@$@#$")]
        public void GetCardByLicensePlateTest2(string licensePlate)
        {
            CardDAL cardDAL = new CardDAL();
            Card card = cardDAL.GetCardByLicensePlate(licensePlate);
            Assert.Null(card);
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