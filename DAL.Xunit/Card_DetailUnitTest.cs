using System;
using Persistence;
using Xunit;
using DAL;

namespace DAL.Xunit
{
    public class Card_DetailUnitTest
    {
        private Card_DetailDAL card_DetailDAL = new Card_DetailDAL();
        [Fact]
        public void GetCardDetailByIDTest1()
        {
            Card_Detail card_Detail = card_DetailDAL.GetCardDetailByID("CM01");
            Assert.NotNull(card_Detail);
            Assert.Equal("CM01", card_Detail.Card_id);
        }
    }
}