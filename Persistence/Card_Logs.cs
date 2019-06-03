using System;

namespace Persistence
{
    public class Card_Logs
    {
        public int Card_id { get; set; }
        public string User_name { get; set; }
        public string LisensePlate { get; set; }
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        public string SendTime { get; set; }
        public double? IntoMoney { get; set; }
        public Card_Logs() { }
        public Card_Logs(int card_id, string user_name, string lisensePlate, DateTime? dateTimeStart, DateTime? dateTimeEnd, string sendTime, double? intoMoney)
        {
            this.Card_id = card_id;
            this.User_name = user_name;
            this.LisensePlate = lisensePlate;
            this.DateTimeStart = dateTimeStart;
            this.DateTimeEnd = dateTimeEnd;
            this.SendTime = sendTime;
            this.IntoMoney = intoMoney;
        }


    }
}