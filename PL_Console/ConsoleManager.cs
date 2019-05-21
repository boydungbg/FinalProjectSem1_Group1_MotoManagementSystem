using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using BL;
using Persistence;
using PL_Console;

namespace PL_console
{
    public class ConsoleManager
    {
        private Menus menu = new Menus();
        string b = "══════════════════════════════════════════════════════════════";
        public void CreateCard()
        {
            char yesNo;
            bool check = false;
            do
            {
                CardBL cardBL = new CardBL();
                CustomerBL cusBL = new CustomerBL();
                Card newCard = new Card();
                Customer newCus = new Customer();
                string card_id = "";
                string customer_id;
                string customer_name;
                string customer_address;
                string customer_licenseplate;
                DateTime start_day = DateTime.Now;
                DateTime end_day = new DateTime(start_day.Year, start_day.Month + 3, start_day.Day);
                Console.Clear();
                Console.WriteLine(b);
                Console.WriteLine(" Tạo thẻ tháng.");
                Console.WriteLine(b);
                try
                {
                    newCard = cardBL.GetCardByWord();
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
                if (newCard == null)
                {
                    card_id = "CM01";
                }
                else
                {
                    int a = Int32.Parse(newCard.Card_id.Substring(3));
                    if (a < 10)
                    {
                        a = a + 1;
                        card_id = Convert.ToString("CM0" + a);
                    }
                    if (a >= 10)
                    {
                        a = a + 1;
                        card_id = Convert.ToString("CM" + a);
                    }
                }
                Console.WriteLine("- Mã thẻ: " + card_id);
                Console.Write("- Nhập số chứng minh thư nhân dân hoặc thẻ căn cước (VD:123456789): ");
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
                        Console.Write("↻ Số chứng minh thư nhân dân hoặc thẻ căn cước đã tồn tại. Nhập lại: ");
                    }
                } while (newCus != null);
                Console.Write("- Nhập tên khách hàng (VD:LE CHI DUNG): ");
                customer_name = validate(3);
                Console.Write("- Nhập địa chỉ (VD:BAC GIANG): ");
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
                        Console.WriteLine(" Tạo thẻ không thành công.");
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
                            throw new Exception("↻ Số chứng minh thư hoặc thẻ căn cước khồn hợp lệ (VD:123456789). Nhập lại: ");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("[0-9]");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {

                        Console.Write("↻ Số chứng minh thư hoặc thẻ căn cước khồn hợp lệ (VD:123456789). Nhập lại: ");
                        continue;
                    }
                    if (input.Length != 9 && input.Length != 12)
                    {
                        Console.Write("↻ Số chứng minh thư hoặc thẻ căn cước khồn hợp lệ (VD:123456789). Nhập lại: ");
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
                            throw new Exception("↻ Tên khách hàng không hợp lệ (VD:LE CHI DUNG). Nhập lại: ");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("^[A-Z]+(([',. -][A-Z ])?[A-Z]*)*$");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {
                        Console.Write("↻ Tên khách hàng hợp lệ (VD:LE CHI DUNG). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 8 || input.Length >= 30)
                    {
                        Console.Write("↻ Tên khách hàng hợp lệ (VD:LE CHI DUNG). Nhập lại: ");
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
                            throw new Exception("↻ Địa chỉ không hợp lệ (VD:BAC GIANG). Nhập lại: ");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Console.Write(e.Message);
                        continue;
                    }
                    Regex regex = new Regex("^[A-Z0-9-]+(([',. ][A-Z 0-9-])?[A-Z0-9-]*)*$");
                    MatchCollection matchCollectionstr = regex.Matches(input);
                    if (matchCollectionstr.Count == 0)
                    {
                        Console.Write("↻ Địa chỉ không hợp lệ (VD:BAC GIANG). Nhập lại: ");
                        continue;
                    }
                    if (input.Length <= 8 || input.Length >= 30)
                    {
                        Console.Write("↻ Địa chỉ không hợp lệ (VD:BAC GIANG). Nhập lại: ");
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
                    if (input.Length <= 9 || input.Length >= 12)
                    {
                        Console.Write("↻ Biển số xe không hợp lệ (VD:88-X8-8888). Nhập lại: ");
                        continue;
                    }
                    break;
                }
                return input;
            }
            // if (check == 6)
            // {
            //     while (true)
            //     {
            //         try
            //         {
            //             input = Console.ReadLine();
            //             if (input == "")
            //             {
            //                 throw new Exception("↻ Vui lòng nhập theo đúng định dạng NĂM-THÁNG-NGÀY (VD:2019-05-20). Nhập lại: ");
            //             }
            //         }
            //         catch (System.Exception e)
            //         {
            //             Console.Write(e.Message);
            //             continue;
            //         }
            //         Regex regex = new Regex("[0-9]{4}[-][0-9]{2}[-][0-9]{2}");
            //         MatchCollection matchCollectionstr = regex.Matches(input);
            //         if (matchCollectionstr.Count == 0)
            //         {
            //             Console.Write("↻ Vui lòng nhập theo đúng định dạng NĂM-THÁNG-NGÀY (VD:2019-05-20). Nhập lại: ");
            //             continue;
            //         }
            //         break;
            //     }
            //     return input;
            // }
            return input;
        }
        public void GetListCardByCardType()
        {
            CardBL cardBL = new CardBL();
            List<Card> listcard = null;
            CustomerBL cusBL = new CustomerBL();
            Customer cus = null;
            string status = "";
            Console.Clear();
            try
            {
                listcard = cardBL.GetListCardByCardType();
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
            if (listcard.Capacity == 0)
            {
                Console.Clear();
                Console.WriteLine("Không có dữ liệu của thẻ!!! Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.MenuManager();
            }
            else
            {
                var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Số CMND/Thẻ căn cước", "Tên khách hàng", "Địa chỉ", "Ngày tạo", "Trạng thái");
                foreach (var card in listcard)
                {
                    try
                    {
                        cusBL = new CustomerBL();
                        cus = cusBL.GetCustomerByLincese_plate(card.LicensePlate);
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

                    foreach (var cardDetail in card.CardDetail)
                    {
                        if (cardDetail.End_day < DateTime.Now)
                        {
                            status = "Ngừng sử dụng";
                        }
                        if (cardDetail.End_day > DateTime.Now)
                        {
                            status = "Được sử dụng";
                        }
                        table.AddRow(card.Card_id, card.LicensePlate, cardDetail.Cus_id,
                         cus.Cus_name, cus.Cus_address, cardDetail.Date_created, status);
                    }
                }
                table.Write(Format.Alternative);
                Console.WriteLine("Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.MenuManager();
            }
        }
    }
}

