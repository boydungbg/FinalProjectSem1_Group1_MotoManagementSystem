using System;
using Persistence;
using Xunit;
using BL;
namespace BL.Xunit
{
    public class Card_DetailUnitTest
    {
        [Fact]
        public void GetCardDetailByIDTest1()
        {
            Card_detailBL card_Detail = new Card_detailBL();
            Assert.NotNull(card_Detail);
        }
    }
}