using System;

namespace Persistence
{
    public class Card_Logs
    {
        public int Card_id { get; set; }
        public string User_name { get; set; }
        public string LisensePlate { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public int? Status { get; set; }
        public double? Money { get; set; }
        public Card_Logs() { }
        public Card_Logs(int card_id, string user_name, string lisensePlate, DateTime? timeIn, DateTime? timeOut, double? money, int? status)
        {
            this.Card_id = card_id;
            this.User_name = user_name;
            this.LisensePlate = lisensePlate;
            this.TimeIn = timeIn;
            this.TimeOut = timeOut;
            this.Status = status;
            this.Money = money;
        }
    }
}