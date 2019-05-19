using System;
namespace Persistence
{
    public class Card_Detail
    {
        public string Card_id { get; set; }
        public string Cus_id { get; set; }
        public DateTime Start_day { get; set; }
        public DateTime End_day { get; set; }
        public DateTime? Date_created { get; set; }

        public Card_Detail()
        {

        }
        public Card_Detail(string card_id, string cus_id, DateTime start_day, DateTime end_day, DateTime? date_created)
        {
            this.Card_id = card_id;
            this.Cus_id = cus_id;
            this.Start_day = start_day;
            this.End_day = end_day;
            this.Date_created = date_created;
        }
    }
}