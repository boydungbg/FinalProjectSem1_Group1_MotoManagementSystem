using DAL;
using Persistence;

namespace BL
{
    public class CardBL
    {
        private CardDAL cardDAL = new CardDAL();
        public bool CreateCard(Card card, Customer cus, Card_Detail card_Detail)
        {
            return cardDAL.CreateCard(card, cus, card_Detail);
        }
        public Card GetCardByID(string id)
        {
            if (id == null)
            {
                return null;
            }
            return cardDAL.GetCardByID(id);
        }
        public bool DeleteCardByID(string cardid, string cusid)
        {
            return cardDAL.DeleteCardByID(cardid,cusid);
        }
    }
}