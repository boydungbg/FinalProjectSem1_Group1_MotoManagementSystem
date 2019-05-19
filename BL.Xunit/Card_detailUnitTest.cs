using Bl;
using Persistence;
using Xunit;

namespace BL.Xunit
{

    public class Card_detailUnitTest
    {
        [Theory]
        [InlineData("CM01")]
        [InlineData("CM02")]
        public void GetCard_detailByIDTest1(string id)
        {
            Card_detailBL card_detailBL = new Card_detailBL();
            Card_Detail card_detail = card_detailBL.GetCard_DetailbyID(id);
            Assert.NotNull(card_detail);
            Assert.Equal(id, card_detail.Card_id);
        }
        [Theory]
        [InlineData("CM99")]
        [InlineData(null)]
        public void GetCard_detailByIDTest2(string id)
        {
            Card_detailBL card_detailBL = new Card_detailBL();
            Card_Detail card_detail = card_detailBL.GetCard_DetailbyID(id);
            Assert.Null(card_detail);
        }
    }
}