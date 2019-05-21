using System;
using DAL;
using Persistence;

namespace BL
{
    public class Card_LogsBL
    {
        private Card_LogsDAL card_LogsDAL = new Card_LogsDAL();
        public bool CreateCardLogs(Card_Logs card_Logs)
        {
            if (card_Logs == null)
            {
                return false;
            }
            return card_LogsDAL.CreateCardLogs(card_Logs);
        }
        public bool DeleteCardLogsByID(string id)
        {
            if (id == null)
            {
                return false;
            }
            return card_LogsDAL.DeleteCardLogsByID(id);
        }
        public Card_Logs GetCardLogsByLisencePlateAndCardID(string LicensePlate, string cardid)
        {
            if (LicensePlate == null)
            {
                return null;
            }
            return card_LogsDAL.GetCardLogsByLicensePlateAndCardID(LicensePlate,cardid);
        }
        public bool UpdateCardLogsByLicensePlateAndCardID(Card_Logs cardLogs ,string licensePlate , string cardid , string dateTimeStart)
        {
             if (licensePlate == null || cardid == null || dateTimeStart == null || cardLogs == null)
            {
                return false;
            }
            return card_LogsDAL.UpdateCardLogsByLicensePlateAndCardID(cardLogs,licensePlate,cardid,dateTimeStart);
        }
    }
}