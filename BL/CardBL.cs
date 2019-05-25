using System.Collections.Generic;
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
            if (cardid == null || cusid == null)
            {
                return false;
            }
            return cardDAL.DeleteCardByID(cardid, cusid);
        }
        public Card GetCardByLicensePlate(string licensePlate)
        {
            if (licensePlate == null)
            {
                return null;
            }
            return cardDAL.GetCardByLicensePlate(licensePlate);
        }
        public bool UpdateCardByID(Card card, string id)
        {
            if (card == null || id == null)
            {
                return false;
            }
            return cardDAL.UpdateCardByID(card, id);
        }
        public List<Card> GetlistCard()
        {
            return cardDAL.GetlistCard();
        }
        public Card GetCardByWord()
        {
            return cardDAL.GetCardByWord();
        }
    }
}