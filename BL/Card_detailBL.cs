using System;
using DAL;
using Persistence;

namespace BL
{
    public class Card_detailBL
    {
        private Card_DetailDAL card_detailDAL = new Card_DetailDAL();
        public Card_Detail GetCardDetailByID(string id)
        {
            if (id == null)
            {
                return null;
            }
            return card_detailDAL.GetCardDetailByID(id);
        }
    }
}