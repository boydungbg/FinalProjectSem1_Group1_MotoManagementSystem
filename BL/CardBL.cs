using DAL;
using Persistence;

namespace BL
{
    public class CardBL
    {
        private CardDAL cardDAL = new CardDAL();
        public bool CreateCard(Card card,Customer cus, Card_Detail card_Detail)
        {
            return cardDAL.CreateCard(card,cus,card_Detail);
        }
    }
}