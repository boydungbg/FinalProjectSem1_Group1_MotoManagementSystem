using DAL;
using Persistence;

namespace BL
{
    public class CustomerBL
    {
        private CustomerDAL customerDAL;
        public CustomerBL()
        {
            customerDAL = new CustomerDAL();
        }
        public Customer GetCustomerByID(string cusid)
        {
            if (cusid == null)
            {
                return null;
            }
            return customerDAL.GetCustomerByID(cusid);
        }
        public Customer GetCustomerByLincese_plate(string lincese_plate)
        {
            if (lincese_plate == null)
            {
                return null;
            }
            return customerDAL.GetCustomerByLincese_plate(lincese_plate);
        }
    }
}