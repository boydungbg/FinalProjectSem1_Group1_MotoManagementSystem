using System;
using System.Collections.Generic;

namespace Persistence
{
    public class Card
    {
        public int? Card_id { get; set; }
        public string LicensePlate { get; set; }
        public string Card_type { get; set; }
        public int? Card_Status { get; set; }
        public DateTime? Date_created { get; set; }
        public List<Card_Logs> CardLogs { get; set; }
        public List<Card_Detail> CardDetail { get; set; }
        public Card()
        {
        }
        public Card(int? card_id, string licensePlate, string card_type, int? card_status, List<Card_Logs> cardLogs, List<Card_Detail> cardDetail, DateTime? date_created)
        {
            this.Card_id = card_id;
            this.LicensePlate = licensePlate;
            this.Card_type = card_type;
            this.Card_Status = card_status;
            this.CardLogs = cardLogs;
            this.CardDetail = cardDetail;
            this.Date_created = date_created;
        }


    }
}