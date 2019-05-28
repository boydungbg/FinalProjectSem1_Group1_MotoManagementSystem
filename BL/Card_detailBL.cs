using DAL;
using Persistence;

namespace BL
{
    public class Card_detailBL
    {
        private Card_detailDAL cardDetailDAL;
        public Card_detailBL()
        {
            cardDetailDAL = new Card_detailDAL();
        }
        public Card_Detail GetCard_DetailbyID(string id)
        {
            if (id == null)
            {
                return null;
            }
            return cardDetailDAL.GetCard_DetailByID(id);
        }
    }
}