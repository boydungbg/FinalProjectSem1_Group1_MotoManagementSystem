using System;
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
            string card_id = "CM21";
            string cus_id = "101029011";
            string cus_name = "Lê Chí Dũng";
            string cus_address = "Bắc Giang";
            string cus_licensePlate = "89-B5-8888";
            DateTime start_day = new DateTime(2019, 05, 17);
            DateTime end_day = new DateTime(2019, 06, 17);
            string cardType = "Thẻ tháng";
            Card_Detail card_Detail = new Card_Detail(card_id, cus_id, start_day, end_day, null);
            Card card = new Card(card_id, cus_licensePlate, cardType, null, null, null);
            Customer cus = new Customer(cus_id, cus_name, cus_address, cus_licensePlate);
            Assert.True(cardBL.CreateCard(card, cus, card_Detail));
            cardBL = new CardBL();
            Assert.True(cardBL.DeleteCardByID("CM21", "101029011"));
        }
        [Fact]
        public void CreateCardTest2()
        {
            CardBL cardBL = new CardBL();
            string card_id = "CM01";
            string cus_id = "123456789";
            string cus_name = "Lê Chí Dũng";
            string cus_address = "Bắc Giang";
            string cus_licensePlate = "89-B5-8888";
            DateTime start_day = new DateTime(2019, 05, 17);
            DateTime end_day = new DateTime(2019, 06, 17);
            string cardType = "Thẻ tháng";
            Card_Detail card_Detail = new Card_Detail(card_id, cus_id, start_day, end_day, null);
            Card card = new Card(card_id, cus_licensePlate, cardType, null, null, null);
            Customer cus = new Customer(cus_id, cus_name, cus_address, cus_licensePlate);
            Assert.False(cardBL.CreateCard(card, cus, card_Detail));
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
        [InlineData("CM40")]
        public void GetCardByIDTest2(string cardid)
        {
            CardBL cardBL = new CardBL();
            Card card = cardBL.GetCardByID(cardid);
            Assert.Null(card);
        }
    }
}