using System;
using System.Collections.Generic;

namespace Persistence
{
    public  class Customer
    {
        public string Cus_id { get; set; }
        public string Cus_name { get; set; }
        public string Cus_address { get; set; }
        public string Cus_licensePlate { get; set; }
        public List<Card_Detail> CardDetail { get; set; }
        public Customer() { }
        public Customer(string cus_id, string cus_name, string cus_address, string cus_lincensePlate, List<Card_Detail> cardDetail)
        {
            this.Cus_id = cus_id;
            this.Cus_name = cus_name;
            this.Cus_address = cus_address;
            this.Cus_licensePlate = cus_lincensePlate;
            this.CardDetail = cardDetail;

        }
    }
}