using System.Collections.Generic;
using DAL;
using Persistence;

namespace BL
{
    public class CardBL
    {
        private CardDAL cardDAL;
        public CardBL()
        {
            cardDAL = new CardDAL();
        }
        public bool CreateCard(Card card, Customer cus)
        {
            if (card == null || cus == null)
            {
                return false;
            }
            return cardDAL.CreateCard(card, cus);
        }
        public Card GetCardByID(int id)
        {
            if (id == 0)
            {
                return null;
            }
            return cardDAL.GetCardByID(id);
        }
        public bool DeleteCardByID(int cardid, string cusid)
        {
            if (cardid == 0 || cusid == null)
            {
                return false;
            }
            return cardDAL.DeleteCardByID(cardid, cusid);
        }
        public bool UpdateCardByID(Card card, int id)
        {
            if (card == null || id == 0)
            {
                return false;
            }
            return cardDAL.UpdateCardByID(card, id);
        }
        public List<Card> GetlistCard(int page)
        {
            return cardDAL.GetlistCard(page);
        }
        public Card GetCardID()
        {
            return cardDAL.GetCardID();
        }
        public double GetCardNo()
        {
            return cardDAL.GetCardNO();
        }
        public List<Card> GetlistCardMonth(int page)
        {
            return cardDAL.GetlistCardMonth(page);
        }
        public double GetCardMonthNo()
        {
            return cardDAL.GetCardMonthNO();
        }
    }
}