using System;
namespace Persistence
{
    class Card_Detail
    {
        public string Card_id { get; set; }
        public string Cus_id { get; set; }
        public string Card_time { get; set; }
        public DateTime Date_created { get; set; }

        public Card_Detail()
        {

        }
        public Card_Detail(string card_id, string cus_id, string card_time, DateTime date_created)
        {
            this.Card_id = card_id;
            this.Cus_id = cus_id;
            this.Card_time = card_time;
            this.Date_created = date_created;
        }
    }
}