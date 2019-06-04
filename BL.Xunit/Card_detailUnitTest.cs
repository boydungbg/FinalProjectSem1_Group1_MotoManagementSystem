using Persistence;
using Xunit;

namespace BL.Xunit
{

    public class Card_detailUnitTest
    {
        [Theory]
        [InlineData(10012)]
        [InlineData(10013)]
        public void GetCard_detailByIDTest1(int id)
        {
            Card_detailBL card_detailBL = new Card_detailBL();
            Card_Detail card_detail = card_detailBL.GetCard_DetailbyID(id);
            Assert.NotNull(card_detail);
            Assert.Equal(id, card_detail.Card_id);
        }
        [Theory]
        [InlineData(101111)]
        [InlineData(0)]
        public void GetCard_detailByIDTest2(int id)
        {
            Card_detailBL card_detailBL = new Card_detailBL();
            Card_Detail card_detail = card_detailBL.GetCard_DetailbyID(id);
            Assert.Null(card_detail);
        }
    }
}