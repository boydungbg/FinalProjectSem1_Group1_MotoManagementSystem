using Persistence;
using Xunit;

namespace DAL.Xunit
{
    public class Card_detailUnitTest
    {
        [Theory]
        [InlineData(10011)]
        [InlineData(10012)]
        public void GetCard_detailTest1(int card_id)
        {
            Card_detailDAL cardDetailDAL = new Card_detailDAL();
            Card_Detail card_detail = cardDetailDAL.GetCard_DetailByID(card_id);
            Assert.NotNull(card_detail);
            Assert.Equal(card_id, card_detail.Card_id);
        }
        [Theory]
        [InlineData(10099)]
        [InlineData(10)]
        public void GetCard_detailTest2(int card_id)
        {
            Card_detailDAL cardDetailDAL = new Card_detailDAL();
            Card_Detail card_detail = cardDetailDAL.GetCard_DetailByID(card_id);
            Assert.Null(card_detail);
        }
    }
}