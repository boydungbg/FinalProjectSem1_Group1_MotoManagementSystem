using Persistence;
using Xunit;

namespace BL.Xunit
{
    public class CustomerUnitTest
    {
        [Theory]
        [InlineData("123456789")]
        [InlineData("122332387")]
        public void GetCustomerByIDTest1(string cusid)
        {
            CustomerBL cusBL = new CustomerBL();
            Customer cus = cusBL.GetCustomerByID(cusid);
            Assert.NotNull(cus);
            Assert.Equal(cusid, cus.Cus_id);
        }
        [Theory]
        [InlineData("121252789")]
        [InlineData(null)]
        public void GetCustomerByIDTest2(string cusid)
        {
            CustomerBL CusBL = new CustomerBL();
            Customer cus = CusBL.GetCustomerByID(cusid);
            Assert.Null(cus);
        }
        [Theory]
        [InlineData("88-G1-8888")]
        [InlineData("22-E1-2222")]
        public void GetCustomerByLincese_plateTest1(string licensePlate)
        {
            CustomerBL cusBL = new CustomerBL();
            Customer cus = cusBL.GetCustomerByLincese_plate(licensePlate);
            Assert.NotNull(cus);
            Assert.Equal(licensePlate, cus.Cus_licensePlate);
        }
        [Theory]
        [InlineData("88-A1-1234")]
        [InlineData(null)]
        public void GetCustomerByLincese_plateTest2(string licensePlate)
        {
            CustomerBL CusBL = new CustomerBL();
            Customer cus = CusBL.GetCustomerByLincese_plate(licensePlate);
            Assert.Null(cus);
        }
    }
}