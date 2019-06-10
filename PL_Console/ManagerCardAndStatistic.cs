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
            card.Start_day = DateTime.Now;
            card.End_day = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day);
            return card;
        }
        public Customer SetInfoCustomer(User user)
        {
            Customer cus = new Customer();
            Console.Write("- Nhập số chứng minh thư nhân dân hoặc thẻ căn cước (VD:123456789): ");
            do
            {
                cus.Cus_id = Console.ReadLine();
                if (validate(2, cus.Cus_id, user) == false)
                {
                    Console.Write("=> Số chứng minh thư hoặc thẻ căn cước khồn hợp lệ (VD:123456789). Nhập lại: ");
                    cus.Cus_id = null;
                }
            } while (cus.Cus_id == null);
            Console.Write("- Nhập tên khách hàng (VD:LE CHI DUNG): ");
            do
            {
                cus.Cus_name = Console.ReadLine().ToUpper();
                if (validate(3, cus.Cus_name, user) == false)
                {
                    Console.Write("=> Tên khách hàng hợp lệ (VD:LE CHI DUNG). Nhập lại: ");
                    cus.Cus_name = null;
                }
            } while (cus.Cus_name == null);
            Console.Write("- Nhập địa chỉ (VD:BAC GIANG): ");
            do
            {
                cus.Cus_address = Console.ReadLine().ToUpper();
                if (validate(6, cus.Cus_address, user) == false)
                {
                    Console.Write("=> Địa chỉ không hợp lệ (VD:BAC GIANG). Nhập lại: ");
                    cus.Cus_address = null;
                }
            } while (cus.Cus_address == null);
            Console.Write("- Nhập biển số xe(VD:88X8-8888): ");
            do
            {
                cus.Cus_licensePlate = Console.ReadLine().ToUpper();
                if (validate(4, cus.Cus_licensePlate, user) == false)
                {
                    Console.Write("=> Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    cus.Cus_licensePlate = null;
                }

            } while (cus.Cus_licensePlate == null);
            return cus;
        }
        public void CreateCard(User user)
        {
            char yesNo;
            do
            {
                bool check = true;
                Card card = SetInfoCard(user);
                Customer cus = SetInfoCustomer(user);
                if (GetCustomerByID(cus.Cus_id, user) != null)
                {
                    Console.WriteLine("=> Số chứng minh thư nhân dân hoặc thẻ căn cước đã tồn tại. (-_-)");
                    check = false;
                }
                if (GetCustomerByLincese_plate(cus.Cus_licensePlate, user) != null)
                {
                    Console.WriteLine("=> Biển số xe đã tồn tại. (-_-)");
                    check = false;
                }
                if (AutoIncrementID(user) > card.Card_id)
                {
                    card.Card_id += 1;
                }
                if (check == true)
                {
                    check = CreateCard(card, cus, user);
                }
                if (check == true)
                {
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("<<<Tạo thẻ thành công.>>>");
                }
                if (check == false)
                {
                    card = null;
                    cus = null;
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("<<<Tạo thẻ không thành công.>>>");
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
        public bool CreateCard(Card card, Customer cus, User user)
        {
            try
            {
                CardBL cardBL = new CardBL();
                cardBL.CreateCard(card, cus);
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
                card_id = 10001;
            }
            else
            {
                card_id = newCard.Card_id.Value + 1;
            }
            return card_id;
        }
        public bool validate(int check, string input, User user)
        {
            if (input == "" || input == null)
            {
                return false;
            }
            if (input == "EXIT" || input == "exit")
            {
                if (user.User_level == 0)
                {
                    menu.MenuManager(user);
                }
                if (user.User_level == 1)
                {
                    menu.MenuSecurity(user);
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
        public List<Card> GetListCards(int page, User user)
        {
            List<Card> listCards = null;
            try
            {
                CardBL cardBL = new CardBL();
                listCards = cardBL.GetlistCard(page);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return listCards;
        }
        public double GetCardNo(User user)
        {
            double cardNo = 0;
            try
            {
                CardBL cardBL = new CardBL();
                cardNo = cardBL.GetCardNo();
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardNo;
        }
        public List<Card> GetListCardMonth(int page, User user)
        {
            List<Card> listCards = null;
            try
            {
                CardBL cardBL = new CardBL();
                listCards = cardBL.GetlistCardMonth(page);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return listCards;
        }
        public double GetCardMonthNo(User user)
        {
            double cardNo = 0;
            try
            {
                CardBL cardBL = new CardBL();
                cardNo = cardBL.GetCardMonthNo();
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardNo;
        }
        public void DisplayListCardsMonth(int page, User user)
        {
            List<Card> listCards = GetListCardMonth(page, user);
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
                        if (item.Card_Status == 0)
                        {
                            status = "Chưa hết hạn";
                        }
                        if (item.Card_Status == 1)
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
                double pageNo = Math.Ceiling(Convert.ToDouble(GetCardMonthNo(user) / 10));
                Console.WriteLine("                                              Trang: {0} / {1}", (page / 10) + 1, pageNo);
                if (pageNo > 1)
                {
                    Console.WriteLine("Nhập mã (24122000) để thoát");
                    Console.Write("Nhập trang: ");
                    do
                    {
                        try
                        {
                            page = Convert.ToInt32(Console.ReadLine());
                            if (page == 24122000)
                            {
                                menu.MenuManager(user);
                            }
                            if (page > pageNo || page <= 0)
                            {
                                Console.Write("Số trang nhập quá lớn hoặc quá nhỏ. Nhập lại: ");
                                page = 0;
                            }
                        }
                        catch (System.Exception)
                        {
                            Console.Write("Số trang nhập không hợp lệ. Nhập lại: ");
                            page = 0;
                        }
                    } while (page == 0);
                    DisplayListCardsMonth(((page - 1) * 10), user);
                }
                else
                {
                    Console.WriteLine("Nhấn Enter để quay lại.");
                    Console.ReadKey();
                }
            }
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
        public Card_Logs GetCardLogsByID(int cardid, int status, User user)
        {
            Card_Logs cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetCardLogsByID(cardid, status);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardLogs;
        }
        public Card_Logs GetCardLogsByLicensePlate(string licensePlate, User user)
        {
            Card_Logs cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetCardLogsByLicensePlate(licensePlate);
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
                cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(cardlogs, cardlogs.LisensePlate, cardlogs.Card_id, cardlogs.TimeIn?.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return true;
        }
        public List<Card_Logs> GetListCardLogsByPage(int page, string from, string to, string type, User user)
        {
            List<Card_Logs> cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetListCardLogsByPage(page, from, to, type);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardLogs;
        }
        public List<Card_Logs> GetListCardLogs(string from, string to, User user)
        {
            List<Card_Logs> cardLogs = null;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogs = cardLogsBL.GetListCardLogs(Convert.ToDateTime(from).ToString("yyyy-MM-dd 00:00:00"), Convert.ToDateTime(to).ToString("yyyy-MM-dd 23:59:59"));
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardLogs;
        }
        public double GetCardLogNO(string from, string to, string type, User user)
        {
            double cardLogsNo = 0;
            try
            {
                Card_LogsBL cardLogsBL = new Card_LogsBL();
                cardLogsNo = cardLogsBL.GetCardLogNO(Convert.ToDateTime(from).ToString("yyyy-MM-dd 00:00:00"), Convert.ToDateTime(to).ToString("yyyy-MM-dd 23:59:59"), type);
            }
            catch (Exception ex)
            {
                CheckUser(user, ex);
            }
            return cardLogsNo;
        }
        public void Statistical(User user, int choose)
        {

            string from;
            string to;
            Console.Clear();
            Console.WriteLine(b);
            if (choose == 1) Console.WriteLine(" Thống kê xe ra vào thẻ ngày");
            if (choose == 2) Console.WriteLine(" Thống kê xe ra vào thẻ tháng");
            Console.WriteLine(b);
            Console.Write("Từ ngày (VD:24/12/2000): ");
            do
            {
                from = Console.ReadLine();
                if (validate(5, from, user) == false)
                {
                    Console.Write("=> Thời gian nhập vào không hợp lệ (VD:24/12/2000). Nhập lại: ");
                    from = null;
                    continue;
                }
                if (Convert.ToDateTime(from) > DateTime.Now || Convert.ToDateTime(from) < new DateTime(2019, 1, 1))
                {
                    Console.Write("=> Thời gian nhập vào phải trước thời gian hiện tại và phải sau năm 2018. Nhập lại: ");
                    from = null;
                }
            } while (from == null);
            Console.Write("Đến ngày (VD:24/12/2019): ");
            do
            {
                to = Console.ReadLine();
                if (validate(5, to, user) == false)
                {
                    Console.Write("=> Thời gian nhập vào không hợp lệ (VD:24/12/2000). Nhập lại: ");
                    to = null;
                    continue;
                }
                if (Convert.ToDateTime(to) < Convert.ToDateTime(from) || Convert.ToDateTime(to) > DateTime.Now)
                {
                    Console.Write("=> Thời gian nhập vào phải trước thời gian bắt đầu và trước hiện tại. Nhập lại: ");
                    to = null;
                }
            } while (to == null);
            if (choose == 1)
            {
                DisplayStatistical(0, from, to, "Thẻ ngày", user);
            }
            if (choose == 2)
            {
                DisplayStatistical(0, from, to, "Thẻ tháng", user);
            }
        }
        public int GetTurn(string from, string to, string type, User user)
        {
            int Count = 0;
            List<Card_Logs> cardLogs = GetListCardLogs(from, to, user);
            foreach (var item in cardLogs)
            {
                Card card = GetCardByID(item.Card_id, user);
                if (item.Status == 1 && card.Card_type == type)
                {
                    Count++;
                }
            }
            return Count;
        }
        public double GetMoney(string from, string to, string type, User user)
        {
            double monney = 0;
            List<Card_Logs> cardLogs = GetListCardLogs(from, to, user);
            foreach (var item in cardLogs)
            {
                Card card = GetCardByID(item.Card_id, user);
                if (item.Status == 1 && card.Card_type == type)
                {
                    monney = monney + Convert.ToDouble(item.Money);
                }
            }
            return monney;
        }
        public void DisplayStatistical(int page, string from, string to, string type, User user)
        {
            string timeOut = "";
            string status = "";
            List<Card_Logs> cardLogs = GetListCardLogsByPage(page, Convert.ToDateTime(from).ToString("yyyy-MM-dd 00:00:00"), Convert.ToDateTime(to).ToString("yyyy-MM-dd 23:59:59"), type, user);
            var table = new ConsoleTable("STT", "Biển số xe", "Thời gian vào", "Thời gian ra", "Mã thẻ", "Loại thẻ", "Trạng thái", "Giá tiền");
            int STT = 0;
            Card card = null;
            foreach (var item in cardLogs)
            {
                card = GetCardByID(item.Card_id, user);
                STT = STT + 1;
                if (item.Status == 0)
                {
                    timeOut = "             ";
                    status = "Chưa lấy xe ";
                }
                if (item.Status == 1)
                {
                    timeOut = Convert.ToString(item.TimeIn);
                    status = "Đã lấy xe ";
                }
                table.AddRow(STT, item.LisensePlate, item.TimeIn, timeOut, item.Card_id, card.Card_type, status, Convert.ToString(item.Money) + " VNĐ");
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
                Console.WriteLine("                   Thổng số tiền: {0} VND                            Số lượt:  {1}", GetMoney(from, to, type, user), GetTurn(from, to, type, user));
                Console.WriteLine();
                Console.WriteLine("                   Số tiền chỉ dành cho các xe dùng {0}.", type);
                Console.WriteLine();
                Console.WriteLine();
                table.Write(Format.Alternative);
                double pageNo = Math.Ceiling(Convert.ToDouble(GetCardLogNO(from, to, type, user) / 10));
                Console.WriteLine("                                              Trang: {0} / {1}", (page / 10) + 1, pageNo);
                if (pageNo > 1)
                {
                    Console.WriteLine("Nhập mã (24122000) để thoát");
                    Console.Write("Nhập trang: ");
                    do
                    {
                        try
                        {
                            page = Convert.ToInt32(Console.ReadLine());
                            if (page == 24122000)
                            {
                                menu.MenuStatictis(user);
                            }
                            if (page > pageNo || page <= 0)
                            {
                                Console.Write("Số trang nhập quá lớn hoặc quá nhỏ. Nhập lại: ");
                                page = 0;
                            }
                        }
                        catch (System.Exception)
                        {
                            Console.Write("Số trang nhập không hợp lệ. Nhập lại: ");
                            page = 0;
                        }
                    } while (page == 0);
                    DisplayStatistical(((page - 1) * 10), from, to, type, user);
                }
                else
                {
                    Console.Write("Nhấn Enter để quay lại.");
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
        }
        public void CheckUser(User user, Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex.Message);
            Console.WriteLine("LỖI!!! MỜI BẠN THỬ LẠI !!!");
            Console.ReadKey();
            if (user.User_level == 0)
            {
                menu.MenuManager(user);
            }
            if (user.User_level == 1)
            {
                menu.MenuSecurity(user);
            }
        }
    }
}

