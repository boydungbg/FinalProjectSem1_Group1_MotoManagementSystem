using System;
using System.Globalization;
using System.Text.RegularExpressions;
using BL;
using Persistence;
using PL_Console;

namespace PL_console
{
    public class ConsoleManager
    {
        string b = "══════════════════════════════════════════════════════════════";
        public void CreateCard()
        {
            Menus menu = new Menus();
            char yesNo;
            bool check = false;
            do
            {
                CardBL cardBL = new CardBL();
                CustomerBL cusBL = new CustomerBL();
                Card newCard = new Card();
                Customer newCus = new Customer();
                string card_id;
                string customer_id;
                string customer_name;
                string customer_address;
                string customer_licenseplate;
                DateTime start_day;
                DateTime end_day;
                Console.Clear();
                Console.WriteLine(b);
                Console.WriteLine(" Tạo thẻ tháng.");
                Console.WriteLine(b);
                Console.Write("- Nhập mã thẻ(VD:CM01): ");
                do
                {
                    card_id = validate(1);
                    try
                    {
                        newCard = cardBL.GetCardByID(card_id);
                    }
                    catch (System.NullReferenceException)
                    {
                        Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                        Console.ReadKey();
                        menu.MenuLogin();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                        Console.ReadKey();
                        menu.MenuLogin();
                    }
                    if (newCard != null)
                    {
                        Console.Write("↻ Mã thẻ đã tồn tại. Nhập lại: ");
                    }
                } while (newCard != null);
                Console.Write("- Nhập số chứng minh thư nhân dân(VD:123456789): ");
                do
                {
                    customer_id = validate(2);
                    try
                    {
                        newCus = cusBL.GetCustomerByID(customer_id);
                    }
                    catch (System.NullReferenceException)
                    {
                        Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                        Console.ReadKey();
                        menu.MenuLogin();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                        Console.ReadKey();
                        menu.MenuLogin();
                    }
                    if (newCus != null)
                    {
                        Console.Write("↻ Mã khách đã tồn tại. Nhập lại: ");
                    }
                } while (newCus != null);
                Console.Write("- Nhập tên khách hàng(VD:Dung dep zai): ");
                customer_name = validate(3);
                Console.Write("- Nhập địa chỉ(VD:Bac Giang): ");
                customer_address = validate(4);
                Console.Write("- Nhập biển số xe(VD:88-X8-8888): ");
                do
                {
                    customer_licenseplate = validate(5);
                    try
                    {
                        newCus = cusBL.GetCustomerByLincese_plate(customer_licenseplate);
                    }
                    catch (System.NullReferenceException)
                    {
                        Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                        Console.ReadKey();
                        menu.MenuLogin();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                        Console.ReadKey();
                        menu.MenuLogin();
                    }
                    if (newCus != null)
                    {
                        Console.Write("↻ Biển số xe đã tồn tại. Nhập lại: ");
                    }
                } while (newCus != null);
                Console.Write("- Nhập ngày thẻ bắt đầu sử dụng(VD:2019-05-20): ");
                do
                {
                    start_day = Convert.ToDateTime(validate(6));
                    if (start_day < DateTime.Now)
                    {
                        Console.Write("↻ Ngày thẻ bắt đầu phải lớn hơn hiện tại. Nhập lại: ");
                    }
                } while (start_day < DateTime.Now);
                Console.Write("- Nhập ngày thẻ ngừng sử dụng(VD:2019-05-20): ");
                do
                {
                    end_day = Convert.ToDateTime(validate(6));
                    if (end_day <= start_day)
                    {
                        Console.Write("↻ Ngày thẻ ngừng sử dụng phải lớn hơn ngày thẻ bắt đầu. Nhập lại: ");
                    }
                } while (end_day <= start_day);
                Card card = new Card(card_id, customer_licenseplate, "Thẻ tháng", null, null, null);
                Customer customer = new Customer(customer_id, customer_name, customer_address, customer_licenseplate);
                Card_Detail card_detail = new Card_Detail(card_id, customer_id, start_day, end_day, null);
                try
                {
                    check = cardBL.CreateCard(card, customer, card_detail);
                    if (check == false)
                    {
                        Console.WriteLine(b);
                        Console.WriteLine();
                        Console.WriteLine("✘ Tạo thẻ không thành công.");
                    }
                    if (check == true)
                    {
                        Console.WriteLine(b);
                        Console.WriteLine();
                        Console.WriteLine("✔ Tạo thẻ thành công.");
                    }
                }
                catch (System.NullReferenceException)
                {
                    Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                    Console.ReadKey();
                    menu.MenuLogin();
                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {
                    Console.WriteLine("MẤT KẾT NỐI, MỜI BẠN ĐĂNG NHẬP LẠI !!!");
                    Console.ReadKey();
                    menu.MenuLogin();
                }
                Console.WriteLine();
                Console.WriteLine(b);
                Console.WriteLine();
                Console.Write("Bạn có muốn tạo thẻ nữa không(Y/N)");
                yesNo = menu.yesNo();
                if (yesNo == 'N')
                {
                    menu.MenuManager();
                }
            } while (yesNo != 'N');
        }
        public string validate(int check)
        {
            string input = "";
            if (check == 1)
            {
                while (true)
                {
                    try
                    {
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            throw new Exception("↻ Mã thẻ không quá 4 kí tự và phải chứa chữ 'CM' ở đầu (VD:CM01). Nhập lại: ");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("[C][MD][0-9]{2}");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {

                        Console.Write("↻ Mã thẻ không quá 4 kí tự và phải chứa chữ 'CM' ở đầu (VD:CM01). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 3 || input.Length >= 5)
                    {
                        Console.Write("↻ Mã thẻ không quá 4 kí tự và phải chứa chữ 'CM' ở đầu (VD:CM01). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            if (check == 2)
            {
                while (true)
                {
                    try
                    {
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            throw new Exception("↻ Số chứng minh thư không được quá 9 số và chỉ có chữ số (VD:123456789). Nhập lại: ");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("[0-9]{9}");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {

                        Console.Write("↻ Số chứng minh thư không được quá 9 số và chỉ có chữ số (VD:123456789). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 8 || input.Length >= 10)
                    {
                        Console.Write("↻ Số chứng minh thư không được quá 9 số và chỉ có chữ số (VD:123456789). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            if (check == 3)
            {
                while (true)
                {
                    try
                    {
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            throw new Exception("↻ Tên khách hàng không hợp lệ (VD:Dung Dep Zai). Nhập lại: ");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {
                        Console.Write("↻ Tên khách hàng hợp lệ (VD:Dung Dep Zai). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 8 || input.Length >= 30)
                    {
                        Console.Write("↻ Tên khách hàng hợp lệ (VD:Dung Dep Zai). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            if (check == 4)
            {
                while (true)
                {
                    try
                    {
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            throw new Exception("↻ Địa chỉ không hợp lệ (VD:Bac Giang). Nhập lại: ");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("^[a-zA-Z0-9-]+(([',. ][a-zA-Z 0-9-])?[a-zA-Z0-9-]*)*$");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {
                        Console.Write("↻ Địa chỉ không hợp lệ(VD:Bac Giang). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 8 || input.Length >= 30)
                    {
                        Console.Write("↻ Địa chỉ không hợp lệ(VD:Bac Giang). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            if (check == 5)
            {
                while (true)
                {
                    try
                    {
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            throw new Exception("↻ Biển số xe không hợp lệ (VD:88-X8-8888). Nhập lại: ");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("^[0-9-]+(([',.-][A-Z0-9-])?[0-9-]*)*$");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {
                        Console.Write("↻ Biển số xe không hợp lệ (VD:88-X8-8888). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 9 || input.Length >= 11)
                    {
                        Console.Write("↻ Biển số xe không hợp lệ (VD:88-X8-8888). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            if (check == 6)
            {
                while (true)
                {
                    try
                    {
                        input = Console.ReadLine();
                        if (input == "")
                        {
                            throw new Exception("↻ Vui lòng nhập theo đúng định dạng NĂM-THÁNG-NGÀY (VD:2019-05-20). Nhập lại: ");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("[0-9]{4}[-][0-9]{2}[-][0-9]{2}");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {
                        Console.Write("↻ Vui lòng nhập theo đúng định dạng NĂM-THÁNG-NGÀY (VD:2019-05-20). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            return input;
        }

    }
}

