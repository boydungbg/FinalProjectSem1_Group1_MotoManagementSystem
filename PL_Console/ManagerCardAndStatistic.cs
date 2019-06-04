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
        public Card SetInfoCard(User user)
        {
            Card card = new Card();
            Console.Clear();
            Console.WriteLine(b);
            Console.WriteLine(" Tạo thẻ tháng.");
            Console.WriteLine(b);
            card.Card_id = AutoIncrementID(user);
            Console.WriteLine("- Mã thẻ: " + card.Card_id);
            card.Card_type = "Thẻ tháng";
            return card;
        }
        public Customer SetInfoCustomer(User user)
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
                if (GetCustomerByID(cus.Cus_id, user) != null)
                {
                    Console.Write("↻ Số chứng minh thư nhân dân hoặc thẻ căn cước đã tồn tại. Nhập lại: ");
                    cus.Cus_id = null;
                }
            } while (cus.Cus_id == null);
            Console.Write("- Nhập tên khách hàng (VD:LE CHI DUNG): ");
            do
            {
                cus.Cus_name = Console.ReadLine().ToUpper();
                if (validate(3, cus.Cus_name) == false)
                {
                    Console.Write("↻ Tên khách hàng hợp lệ (VD:LE CHI DUNG). Nhập lại: ");
                    cus.Cus_name = null;
                }
            } while (cus.Cus_name == null);
            Console.Write("- Nhập địa chỉ (VD:BAC GIANG): ");
            do
            {
                cus.Cus_address = Console.ReadLine().ToUpper();
                if (validate(6, cus.Cus_address) == false)
                {
                    Console.Write("↻ Địa chỉ không hợp lệ (VD:BAC GIANG). Nhập lại: ");
                    cus.Cus_address = null;
                }
            } while (cus.Cus_address == null);
            Console.Write("- Nhập biển số xe(VD:88X8-8888): ");
            do
            {
                cus.Cus_licensePlate = Console.ReadLine().ToUpper();
                if (validate(4, cus.Cus_licensePlate) == false)
                {
                    Console.Write("↻ Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    cus.Cus_licensePlate = null;
                }
                if (GetCustomerByLincese_plate(cus.Cus_licensePlate, user) != null)
                {
                    Console.Write("↻ Biển số xe đã tồn tại. Nhập lại: ");
                    cus.Cus_licensePlate = null;
                }
            } while (cus.Cus_licensePlate == null);
            return cus;
        }
        public void CreateCard(User user)
        {
            bool check = false;
            char yesNo;
            do
            {
                Card card = SetInfoCard(user);
                Customer cus = SetInfoCustomer(user);
                Card_Detail cd = new Card_Detail();
                cd.Start_day = DateTime.Now;
                cd.End_day = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day);
                check = CreateCard(card, cus, cd, user);
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
                    menu.MenuManager(user);
                }
            } while (yesNo != 'N'); ;

        }
        public bool CreateCard(Card card, Customer cus, Card_Detail cd, User user)
        {
            try
            {
                CardBL cardBL = new CardBL();
                cardBL.CreateCard(card, cus, cd);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return true;
        }
        public Customer GetCustomerByLincese_plate(string licensePlate, User user)
        {
            Customer newcus = null;
            try
            {
                CustomerBL cusBL = new CustomerBL();
                newcus = cusBL.GetCustomerByLincese_plate(licensePlate);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return newcus;
        }
        public Customer GetCustomerByID(string customer_id, User user)
        {
            Customer newCus = null;
            try
            {
                CustomerBL cusBL = new CustomerBL();
                newCus = cusBL.GetCustomerByID(customer_id);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);

            }
            return newCus;
        }
        public int AutoIncrementID(User user)
        {
            int card_id = 0;
            Card newCard = null;
            try
            {
                CardBL cardBL = new CardBL();
                newCard = cardBL.GetCardID();
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            if (newCard == null)
            {
                card_id = 10000;
            }
            else
            {
                card_id = newCard.Card_id.Value + 1;
            }
            return card_id;
        }
        public bool validate(int check, string input)
        {
            if (input == "" || input == null)
            {
                return false;
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
                if (input.Length <= 8 || input.Length >= 30)
                {
                    return false;
                }
            }
            if (check == 6)
            {
                Regex regex = new Regex("[A-Z0-9 -]*\\S$");
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
            if (check == 5)
            {
                Regex regex = new Regex("^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[1,3-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$");
                MatchCollection matchCollectionstr = regex.Matches(input);
                if (matchCollectionstr.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public List<Card> GetListCards(User user)
        {
            List<Card> listCards = null;
            try
            {
                CardBL cardBL = new CardBL();
                listCards = cardBL.GetlistCard();
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return listCards;
        }
        public void DisplayListCardsMonth(User user)
        {
            List<Card> listCards = GetListCards(user);
            string status = "";
            int Count = 0;
            Console.Clear();
            if (listCards.Capacity == 0)
            {
                Console.WriteLine("Không có dữ liệu của thẻ!!! Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.MenuManager(user);
            }
            else
            {
                var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Số CMND/Thẻ căn cước", "Tên khách hàng", "Địa chỉ", "Ngày tạo", "Trạng thái");
                foreach (var item in listCards)
                {
                    if (item.Card_type == "Thẻ tháng")
                    {
                        Customer cus = GetCustomerByLincese_plate(item.LicensePlate, user);
                        Card_Detail cd = GetCardDetailByID(item.Card_id, user);
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
                    menu.MenuManager(user);
                }
                table.Write(Format.Alternative);
                Console.WriteLine("Nhấn Enter để quay lại.");
                Console.ReadKey();
            }
        }
        public Card_Detail GetCardDetailByID(int? cardid, User user)
        {
            Card_Detail cd = null;
            try
            {
                Card_detailBL cdBL = new Card_detailBL();
                cd = cdBL.GetCard_DetailbyID(cardid.Value);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cd;
        }
        public Card_Logs GetCardLogsByCardIDAndLicensePlate(int cardid, string licensePlate, User user)
        {
            Card_Logs cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetCardLogsByCardIDAndLicensePlate(cardid, licensePlate);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardLogs;
        }
        public Card GetCardByID(int cardid, User user)
        {
            Card card = null;
            try
            {
                CardBL cardBL = new CardBL();
                card = cardBL.GetCardByID(cardid);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return card;
        }
        public Card GetCardByLicensePlate(string licensePlate, User user)
        {
            Card newCard = null;
            try
            {
                CardBL cardBL = new CardBL();
                newCard = cardBL.GetCardByLicensePlate(licensePlate);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return newCard;
        }
        public bool UpdateCardByID(Card card, User user)
        {
            try
            {
                CardBL cardBL = new CardBL();
                cardBL.UpdateCardByID((card), card.Card_id.Value);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return true;
        }
        public bool CreateCardLogs(Card_Logs cardlogs, User user)
        {
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogsBL.CreateCardLogs(cardlogs);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return true;
        }
        public bool UpdateCardLogs(Card_Logs cardlogs, User user)
        {
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(cardlogs, cardlogs.LisensePlate, cardlogs.Card_id, cardlogs.DateTimeStart?.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return true;
        }
        public List<Card_Logs> GetListCardLogs(string from, string to, User user)
        {
            List<Card_Logs> cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetListCardLogs(from, to);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardLogs;
        }
        public void Statistical(User user)
        {
            string datimeEnd = "";
            string status = "";
            string intoMoney = "";
            string from;
            string to;
            double totalmoney = 0;
            int Inturn = 0;
            Console.Clear();
            Console.WriteLine(b);
            Console.WriteLine(" Thống kê");
            Console.WriteLine(b);
            Console.Write("Từ ngày (VD:24/12/2000): ");
            do
            {
                from = Console.ReadLine();
                if (validate(5, from) == false)
                {
                    Console.Write("Thời gian nhập vào không hợp lệ (VD:24/12/2000). Nhập lại: ");
                    from = null;
                    continue;
                }
                try
                {
                    if (Convert.ToDateTime(from) > DateTime.Now || Convert.ToDateTime(from) < new DateTime(2018, 1, 1))
                    {
                        Console.Write("Thời gian nhập vào phải sau thời gian hiện tại và phải trước năm 2018. Nhập lại: ");
                        from = null;
                    }
                }
                catch (System.Exception)
                {
                    Console.Write("Thời gian nhập vào không hợp lệ (VD:24/12/2000). Nhập lại: ");
                    from = null;
                }
            } while (from == null);
            Console.Write("Đến ngày (VD:24/12/2019): ");
            do
            {
                to = Console.ReadLine();
                if (validate(5, from) == false)
                {
                    Console.Write("Thời gian không hợp lệ (VD:24/12/2019). Nhập lại: ");
                    to = null;
                    continue;
                }
                try
                {
                    if (Convert.ToDateTime(to) < Convert.ToDateTime(from))
                    {
                        Console.Write("Thời gian nhập vào phải trước thời gian bắt đầu. Nhập lại: ");
                        to = null;
                    }
                }
                catch (System.Exception)
                {
                    Console.Write("Thời gian không hợp lệ (VD:24/12/2019). Nhập lại: ");
                    to = null;
                }
            } while (to == null);
            List<Card_Logs> cardLogs = GetListCardLogs(Convert.ToDateTime(from).ToString("yyyy-MM-dd 00:00:00"), Convert.ToDateTime(to).ToString("yyyy-MM-dd 23:59:59"), user);
            var table = new ConsoleTable("STT", "Biển số xe", "Thời gian vào", "Thời gian ra", "Mã thẻ", "Loại thẻ", "Trạng thái", "Giá tiền");
            int STT = 0;
            Card card = null;
            foreach (var item in cardLogs)
            {
                card = GetCardByID(item.Card_id, user);
                if (card.Card_type == "Thẻ ngày" || item.IntoMoney > 0)
                {
                    STT = STT + 1;
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
            }
            if (STT <= 0)
            {
                Console.WriteLine("Không có dữ liệu nào về xe ra xe vào! Nhấn Enter để quay lại.");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("                   Từ ngày: {0}                                   Đến ngày: {1}", from, to);
                Console.WriteLine();
                Console.WriteLine("                   Thổng số tiền: {0} VND                            Số lượt:  {1}", totalmoney, Inturn);
                Console.WriteLine();
                Console.WriteLine("                   Số tiền chỉ dành cho các xe dùng thẻ ngày và thẻ tháng đã hết hạn.");
                Console.WriteLine();
                Console.WriteLine();
                table.Write(Format.Alternative);
                Console.Write("Nhấn Enter để quay lại.");
                Console.ReadKey();
                Console.WriteLine();
            }
        }
        public void CheckUser(User user, Exception ex)
        {

            Translate tran = new Translate();
            Console.WriteLine();
            Console.WriteLine(tran.Translator(Convert.ToString(ex.Message)));
            Console.WriteLine("LỖI!!! MỜI BẠN THỬ LẠI !!!");
            Console.ReadKey();
            if (user.User_level == 0)
            {
                menu.MenuManager(user);
            }
            if (user.User_level == 1)
            {
                menu.CheckInCheckOut(user);
            }
        }
    }
}

