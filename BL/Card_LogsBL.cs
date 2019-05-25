using System;
using System.Collections.Generic;
using DAL;
using Persistence;

namespace BL
{
    public class Card_LogsBL
    {
        private Card_LogsDAL card_LogsDAL = new Card_LogsDAL();
        public bool CreateCardLogs(Card_Logs card_Logs)
        {

            return card_LogsDAL.CreateCardLogs(card_Logs);
        }
        public bool DeleteCardLogsByID(string id)
        {
            return card_LogsDAL.DeleteCardLogsByID(id);
        }
        public Card_Logs GetCardLogsByLisencePlateAndCardID(string LicensePlate, string cardid)
        {
            return card_LogsDAL.GetCardLogsByLicensePlateAndCardID(LicensePlate, cardid);
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs, string licensePlate, string cardid, string dateTimeStart)
        {

            return card_LogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs, licensePlate, cardid, dateTimeStart);
        }
        public List<Card_Logs> GetListCardLogs(string from, string to)
        {
            return card_LogsDAL.GetListCardLogs(from, to);
        }
    }
}