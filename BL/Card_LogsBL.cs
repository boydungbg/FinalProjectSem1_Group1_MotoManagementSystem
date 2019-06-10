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
        public Card_Logs GetCardLogsByCardIDAndLicensePlate(int cardid, string licensePlate)
        {
            if (cardid == 0 || licensePlate == null)
            {
                return null;
            }
            return card_LogsDAL.GetCardLogsByCardIDAndLicensePlate(cardid, licensePlate);
        }
        public Card_Logs GetCardLogsByID(int cardid, int status)
        {
            if (cardid == 0)
            {
                return null;
            }
            return card_LogsDAL.GetCardLogsByID(cardid, status);
        }
        public Card_Logs GetCardLogsByLicensePlate(string licensePlate)
        {
            if (licensePlate == null)
            {
                return null;
            }
            return card_LogsDAL.GetCardLogsByLicensePlate(licensePlate);
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs, string licensePlate, int cardid, string timeIn)
        {
            if (cardLogs == null || licensePlate == null || cardid == 0 || timeIn == null)
            {
                return false;
            }
            return card_LogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, licensePlate, cardid, timeIn);
        }
        public List<Card_Logs> GetListCardLogsByPage(int page, string from, string to, string type)
        {
            if (from == null || to == null || type == null || page < 0)
            {
                return null;
            }
            return card_LogsDAL.GetListCardLogsByPage(page, from, to, type);
        }
        public List<Card_Logs> GetListCardLogs(string from, string to)
        {
            if (from == null || to == null)
            {
                return null;
            }
            return card_LogsDAL.GetListCardLogs(from, to);
        }
        public double GetCardLogNO(string from, string to, string type)
        {
            return card_LogsDAL.GetCardLogNO(from, to, type);
        }
    }
}