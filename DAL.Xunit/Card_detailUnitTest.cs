using Persistence;
using Xunit;

namespace DAL.Xunit
{
    public class Card_detailUnitTest
    {
        [Theory]
        [InlineData("CM01")]
        [InlineData("CM02")]
        public void GetCard_detailTest1(string card_id)
        {
            Card_detailDAL cardDetailDAL = new Card_detailDAL();
            Card_Detail card_detail = cardDetailDAL.GetCard_DetailByID(card_id);
            Assert.NotNull(card_detail);
            Assert.Equal(card_id, card_detail.Card_id);
        }
        [Theory]
        [InlineData("CM99")]
        [InlineData(null)]
        public void GetCard_detailTest2(string card_id)
        {
            Card_detailDAL cardDetailDAL = new Card_detailDAL();
            Card_Detail card_detail = cardDetailDAL.GetCard_DetailByID(card_id);
            Assert.Null(card_detail);
        }
    }
}