using Persistence;
using Xunit;

namespace DAL.Xunit
{
    public class CustomerUnitTest
    {
        [Theory]
        [InlineData("123456789")]
        [InlineData("122332387")]

        public void GetCustomerByIDTest1(string cusid)
        {
            CustomerDAL CusDAL = new CustomerDAL();
            Customer cus = CusDAL.GetCustomerByID(cusid);
            Assert.NotNull(cus);
            Assert.Equal(cusid, cus.Cus_id);
            
        }
        [Theory]
        [InlineData("121252789")]
        [InlineData("113454789")]
        public void GetCustomerByIDTest2(string cusid)
        {
            CustomerDAL CusDAL = new CustomerDAL();
            Customer cus = CusDAL.GetCustomerByID(cusid);
            Assert.Null(cus);
        }
        [Theory]
        [InlineData("88A1-8888")]
        [InlineData("75G1-2222")]
        public void GetCustomerByLincese_plateTest1(string licensePlate)
        {
            CustomerDAL CusDAL = new CustomerDAL();
            Customer cus = CusDAL.GetCustomerByLincese_plate(licensePlate);
            Assert.NotNull(cus);
            Assert.Equal(licensePlate, cus.Cus_licensePlate);
        }
        [Theory]
        [InlineData("88A1-1234")]
        [InlineData(null)]
        public void GetCustomerByLincese_plateTest2(string licensePlate)
        {
            CustomerDAL CusDAL = new CustomerDAL();
            Customer cus = CusDAL.GetCustomerByLincese_plate(licensePlate);
            Assert.Null(cus);
        }
    }
}