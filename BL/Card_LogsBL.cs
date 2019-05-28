using System;
using System.Collections.Generic;
using DAL;
using Persistence;

namespace BL
{
    public class Card_LogsBL
    {
        private Card_LogsDAL card_LogsDAL;
        public Card_LogsBL()
        {
            card_LogsDAL = new Card_LogsDAL();
        }
        public bool CreateCardLogs(Card_Logs card_Logs)
        {
            if (card_Logs == null)
            {
                return false;
            }
            return card_LogsDAL.CreateCardLogs(card_Logs);
        }
        public Card_Logs GetCardLogsByCardIDAndLicensePlate(string cardid, string licensePlate)
        {
            if (cardid == null || licensePlate == null)
            {
                return null;
            }
            return card_LogsDAL.GetCardLogsByCardIDAndLicensePlate(cardid, licensePlate);
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs, string licensePlate, string cardid, string dateTimeStart)
        {
            if (cardLogs == null || licensePlate == null || cardid == null || dateTimeStart == null)
            {
                return false;
            }
            return card_LogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, licensePlate, cardid, dateTimeStart);
        }
        public List<Card_Logs> GetListCardLogs(string from, string to)
        {
            if (from == null || to == null)
            {
                return null;
            }
            return card_LogsDAL.GetListCardLogs(from, to);
        }
    }
}