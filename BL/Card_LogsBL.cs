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
        public Card_Logs GetCardLogsByLisencePlate(string LicensePlate)
        {
            if (LicensePlate == null)
            {
                return null;
            }
            return card_LogsDAL.GetCardLogsByLicensePlate(LicensePlate);
        }
    }
}