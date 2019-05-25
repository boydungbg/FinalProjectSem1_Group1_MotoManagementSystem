using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using BL;
using Persistence;
using PL_Console;

namespace PL_console
{
    public class ManagerCardAndStatistic
    {
        private Menus menu = new Menus();
        string b = "══════════════════════════════════════════════════════════════";
        public Card SetInfoCard()
        {
            Card card = new Card();

            Console.Clear();
            Console.WriteLine(b);
            Console.WriteLine(" Tạo thẻ tháng.");
            Console.WriteLine(b);
            card.Card_id = GetCardByWord();
            Console.WriteLine("- Mã thẻ: " + card.Card_id);
            card.Card_type = "Thẻ tháng";
            return card;
        }
        public Customer SetInfoCustomer()
        {
            Customer cus = new Customer();
            Console.Write("- Nhập số chứng minh thư nhân dân hoặc thẻ căn cước (VD:123456789): ");
            do
            {
                cus.Cus_id = Console.ReadLine();
                if (validate(2, cus.Cus_id) == false)
                {
                    Console.Write("↻ Số chứng minh thư hoặc thẻ căn cước khồn hợp lệ (VD:123456789). Nhập lại: ");
                    cus.Cus_id = null;
                }
                if (GetCustomerByID(cus.Cus_id) != null)
                {
                    Console.Write("↻ Số chứng minh thư nhân dân hoặc thẻ căn cước đã tồn tại. Nhập lại: ");
                    cus.Cus_id = null;
                }
            } while (cus.Cus_id == null);
            Console.Write("- Nhập tên khách hàng (VD:LE CHI DUNG): ");
            do
            {
                cus.Cus_name = Console.ReadLine();
                if (validate(3, cus.Cus_name) == false)
                {
                    Console.Write("↻ Tên khách hàng hợp lệ (VD:LE CHI DUNG). Nhập lại: ");
                    cus.Cus_name = null;
                }
            } while (cus.Cus_name == null);
            Console.Write("- Nhập địa chỉ (VD:BAC GIANG): ");
            do
            {
                cus.Cus_address = Console.ReadLine();
                if (validate(3, cus.Cus_address) == false)
                {
                    Console.Write("↻ Địa chỉ không hợp lệ (VD:BAC GIANG). Nhập lại: ");
                    cus.Cus_address = null;
                }
            } while (cus.Cus_address == null);
            Console.Write("- Nhập biển số xe(VD:88X8-8888): ");
            do
            {
                cus.Cus_licensePlate = Console.ReadLine();
                if (validate(4, cus.Cus_licensePlate) == false)
                {
                    Console.Write("↻ Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    cus.Cus_licensePlate = null;
                }
                if (GetCustomerByLincese_plate(cus.Cus_licensePlate) != null)
                {
                    Console.Write("↻ Biển số xe đã tồn tại. Nhập lại: ");
                    cus.Cus_licensePlate = null;
                }
            } while (cus.Cus_licensePlate == null);
            return cus;
        }
        public void CreateCard()
        {
            bool check = false;
            char yesNo;
            do
            {
                Card card = SetInfoCard();
                Customer cus = SetInfoCustomer();
                Card_Detail cd = new Card_Detail();
                cd.Start_day = DateTime.Now;
                cd.End_day = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day);
                check = CheckCreateCard(card, cus, cd);
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
                Console.WriteLine();
                Console.WriteLine(b);
                Console.WriteLine();
                Console.Write("Bạn có muốn tạo thẻ nữa không(Y/N)");
                yesNo = menu.yesNo();
                if (yesNo == 'N')
                {
                    menu.MenuManager();
                }
            } while (yesNo != 'N'); ;

        }
        public bool CheckCreateCard(Card card, Customer cus, Card_Detail cd)
        {
            try
            {
                CardBL cardBL = new CardBL();
                cardBL.CreateCard(card, cus, cd);
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
            return true;
        }
        public Customer GetCustomerByLincese_plate(string licensePlate)
        {
            Customer newcus = new Customer();
            try
            {
                CustomerBL cusBL = new CustomerBL();
                newcus = cusBL.GetCustomerByLincese_plate(licensePlate);
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
            return newcus;
        }
        public Customer GetCustomerByID(string customer_id)
        {
            Customer newCus = null;
            try
            {
                CustomerBL cusBL = new CustomerBL();
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
            return newCus;
        }
        public string GetCardByWord()
        {
            string card_id = "";
            Card newCard = null;
            try
            {
                CardBL cardBL = new CardBL();
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
                int a = Int32.Parse(newCard.Card_id.Substring(2));
                if (a < 9)
                {
                    a = a + 1;
                    card_id = Convert.ToString("CM0" + a);
                }
                if (a >= 9)
                {
                    a = a + 1;
                    card_id = Convert.ToString("CM" + a);
                }
            }
            return card_id;
        }
        public bool validate(int check, string input)
        {
            if (input == "" || input == null)
            {
                return false;
            }
            if (check == 1)
            {
                Regex regex = new Regex("[C][MD][0-9]{2}");
                MatchCollection matchCollectionstr = regex.Matches(input);
                if (matchCollectionstr.Count == 0)
                {
                    return false;
                }
                if (input.Length <= 3 || input.Length >= 5)
                {
                    return false;
                }
            }
            if (check == 2)
            {
                Regex regex = new Regex("[0-9]");
                MatchCollection matchCollectionstr = regex.Matches(input);
                if (matchCollectionstr.Count == 0)
                {
                    return false;
                }
                if (input.Length != 9 && input.Length != 12)
                {
                    return false;
                }
            }
            if (check == 3)
            {
                Regex regex = new Regex("^[A-Z]+(([',. -][A-Z ])?[A-Z]*)*$");
                MatchCollection matchCollectionstr = regex.Matches(input);
                if (matchCollectionstr.Count == 0)
                {
                    return false;
                }
                if (input.Length <= 8 || input.Length >= 60)
                {
                    return false;
                }
            }
            if (check == 4)
            {
                Regex regex = new Regex("^[0-9]+[A-Z0-9]+[-]+(([0-9])?[.]?[0-9]*)*$");
                MatchCollection matchCollectionstr = regex.Matches(input);
                if (matchCollectionstr.Count == 0)
                {
                    return false;
                }
                if (input.Length != 9 && input.Length != 11)
                {
                    return false;
                }
            }
            return true;
        }
        public List<Card> GetListCards()
        {
            List<Card> listCards = null;
            try
            {
                CardBL cardBL = new CardBL();
                listCards = cardBL.GetlistCard();
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
            return listCards;
        }
        public void DisplayListCardsMonth()
        {
            List<Card> listCards = GetListCards();
            string status = "";
            int Count = 0;
            Console.Clear();
            if (listCards.Capacity == 0)
            {
                Console.WriteLine("Không có dữ liệu của thẻ!!! Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.MenuManager();
            }
            else
            {
                var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Số CMND/Thẻ căn cước", "Tên khách hàng", "Địa chỉ", "Ngày tạo", "Trạng thái");
                foreach (var item in listCards)
                {
                    if (item.Card_type == "Thẻ tháng")
                    {
                        Customer cus = GetCustomerByLincese_plate(item.LicensePlate);
                        Card_Detail cd = GetCardDetailByID(item.Card_id);
                        if (cd.End_day > DateTime.Now)
                        {
                            status = "Chưa hết hạn";
                        }
                        if (cd.End_day < DateTime.Now)
                        {
                            status = "Hết hạn";
                        }
                        table.AddRow(item.Card_id, item.LicensePlate, cus.Cus_id,
                         cus.Cus_name, cus.Cus_address, item.Date_created, status);
                        Count++;
                    }
                }
                if (Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Không có dữ liệu của thẻ tháng!!! Nhấn Enter để quay lại.");
                    Console.ReadKey();
                    menu.MenuManager();
                }
                table.Write(Format.Alternative);
                Console.WriteLine("Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.MenuManager();
            }
        }
        public Card_Detail GetCardDetailByID(string cardid)
        {
            Card_Detail cd = null;
            try
            {
                Card_detailBL cdBL = new Card_detailBL();
                cd = cdBL.GetCard_DetailbyID(cardid);
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
            return cd;
        }
        public Card_Logs GetCardLogsByLisencePlateAndCardID(string licensePlate, string cardid)
        {
            Card_Logs cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetCardLogsByLisencePlateAndCardID(licensePlate, cardid);
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
            return cardLogs;
        }
        public Card GetCardByID(string cardid)
        {
            Card card = null;
            try
            {
                CardBL cardBL = new CardBL();
                card = cardBL.GetCardByID(cardid);
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
            return card;
        }
        public Card GetCardByLicensePlate(string licensePlate)
        {
            Card newCard = null;
            try
            {
                CardBL cardBL = new CardBL();
                newCard = cardBL.GetCardByLicensePlate(licensePlate);
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
            return newCard;
        }
        public bool UpdateCardByID(Card card)
        {
            try
            {
                CardBL cardBL = new CardBL();
                cardBL.UpdateCardByID((card), card.Card_id);
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
            return true;
        }
        public bool CreateCardLogs(Card_Logs cardlogs)
        {
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogsBL.CreateCardLogs(cardlogs);
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
            return true;
        }
        public bool UpdateCardLogs(Card_Logs cardlogs)
        {
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(cardlogs, cardlogs.LisensePlate, cardlogs.Card_id, cardlogs.DateTimeStart?.ToString("yyyy-MM-dd HH:mm:ss"));
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
            return true;
        }
        public List<Card_Logs> GetListCardLogs(string from, string to)
        {
            List<Card_Logs> cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetListCardLogs(from, to);
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
            return cardLogs;
        }
        public void Statistical()
        {
            string datimeEnd = "";
            string status = "";
            string intoMoney = "";
            double totalmoney = 0;
            int Inturn = 0;
            Console.Clear();
            Console.WriteLine("Từ: ");
            string from = Console.ReadLine();
            Console.WriteLine("Đến: ");
            string to = Console.ReadLine();
            List<Card_Logs> cardLogs = GetListCardLogs(from, to);
            var table = new ConsoleTable("STT", "Biển số xe", "Thời gian vào", "Thời gian ra", "Mã thẻ", "Loại thẻ", "Trạng thái", "Giá tiền");
            int STT = 0;
            Card card = null;
            foreach (var item in cardLogs)
            {
                STT += 1;
                card = GetCardByID(item.Card_id);
                if (item.DateTimeEnd == new DateTime(0))
                {
                    datimeEnd = "             ";
                    status = "Chưa lấy xe ";
                    intoMoney = "             ";
                }
                if (item.DateTimeEnd != new DateTime(0))
                {
                    datimeEnd = Convert.ToString(item.DateTimeEnd);
                    status = "Đã lấy xe ";
                    intoMoney = Convert.ToString(item.IntoMoney) + "  VNĐ";
                    totalmoney = totalmoney + Convert.ToDouble(item.IntoMoney);
                    Inturn++;
                }
                table.AddRow(STT, item.LisensePlate, item.DateTimeStart, datimeEnd, item.Card_id, card.Card_type, status, intoMoney);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                        Thổng số tiền: {0}              Số lượt:  {1}", totalmoney, Inturn);
            table.Write(Format.Alternative);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Nhấn Enter để tiếp tục");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}

