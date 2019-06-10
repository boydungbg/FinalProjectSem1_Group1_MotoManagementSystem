using System;
using System.Collections.Generic;

namespace Persistence
{
    public class Card
    {
        public int? Card_id { get; set; }
        public string Cus_id { get; set; }
        public string LicensePlate { get; set; }
        public int? Card_Status { get; set; }
        public DateTime? Start_day { get; set; }
        public DateTime? End_day { get; set; }
        public string Card_type { get; set; }
        public List<Card_Logs> CardLogs { get; set; }
        public DateTime? Date_created { get; set; }
        public Card()
        {
        }
        public Card(int? card_id, string cus_id, string licensePlate, string card_type, int? card_status, DateTime? start_day, DateTime? end_day, List<Card_Logs> cardLogs, DateTime? date_created)
        {
            this.Card_id = card_id;
            this.Cus_id = cus_id;
            this.Start_day = start_day;
            this.End_day = end_day;
            this.LicensePlate = licensePlate;
            this.Card_type = card_type;
            this.Card_Status = card_status;
            this.CardLogs = cardLogs;
            this.Date_created = date_created;
        }


    }
}