using DAL;
using Persistence;

namespace Bl
{
    public class Card_detailBL
    {
        private Card_detailDAL cardDetailDAL = new Card_detailDAL();
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