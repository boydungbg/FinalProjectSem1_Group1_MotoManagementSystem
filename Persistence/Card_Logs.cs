using System;

namespace Persistence
{
    public class Card_Logs
    {
        public string Card_id { get; set; }
        public string User_id { get; set; }
        public string LisensePlate { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string SendTime { get; set; }
        public double IntoMoney { get; set; }
        public Card_Logs() { }
        public Card_Logs(string card_id, string user_id, string lisensePlate, DateTime dateTimeStart, DateTime dateTimeEnd, string sendTime, double intoMoney)
        {
            this.Card_id = card_id;
            this.User_id = user_id;
            this.LisensePlate = lisensePlate;
            this.DateTimeStart = dateTimeStart;
            this.DateTimeEnd = dateTimeEnd;
            this.SendTime = sendTime;
            this.IntoMoney = intoMoney;
        }


    }
}