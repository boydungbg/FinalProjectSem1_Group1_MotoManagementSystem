using System;
using System.Collections.Generic;
using BL;
using Persistence;
using PL_console;
namespace PL_Console
{
    public class SecurityCheckInCheckOut
    {
        string b = "══════════════════════════════════════════════════════════════";
        private ManagerCardAndStatistic manager = new ManagerCardAndStatistic();
        private Menus menu = new Menus();
        public void DisplayListCards(int page, int choose, User user)
        {
            List<Card> ListCards = manager.GetListCards(page, user);
            if (ListCards.Capacity == 0)
            {
                Console.Clear();
                Console.WriteLine("Không có dữ liệu của thẻ!!! Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.MenuSecurity(user);
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                string status = "";
                string timeIn = "";
                var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Loại thẻ", "Trạng thái", "Thời gian xe vào");
                Card_Logs cardLogs = null;
                foreach (var item in ListCards)
                {
                    cardLogs = manager.GetCardLogsByCardIDAndLicensePlate(item.Card_id.Value, item.LicensePlate, user);
                    if (cardLogs != null && cardLogs.Status == 0)
                    {
                        status = "Hoạt động";
                        timeIn = Convert.ToString(cardLogs.TimeIn);
                    }
                    if (cardLogs == null || cardLogs.Status == 1)
                    {
                        status = "Không hoạt động";
                        timeIn = "Không có";
                    }
                    table.AddRow(item.Card_id, item.LicensePlate, item.Card_type, status, timeIn);
                }
                table.Write(Format.Alternative);
                double pageNo = Math.Ceiling(Convert.ToDouble(manager.GetCardNo(user) / 10));
                Console.WriteLine("                               Trang: {0} / {1}", (page / 10) + 1, pageNo);
                if (pageNo > 1)
                {
                    Console.WriteLine("Nhập mã (24122000) để tiếp tục");
                    Console.Write("Nhập trang: ");
                    do
                    {
                        try
                        {
                            page = Convert.ToInt32(Console.ReadLine());
                            if (page == 24122000)
                            {
                                break;
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
                    if (page != 24122000)
                    {
                        DisplayListCards(((page - 1) * 10), choose, user);
                    }
                }
                else
                {
                    Console.WriteLine("Nhấn Enter để quay lại.");
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
        }
        public Card EnterCardID(int check, User user)
        {
            int card_id;
            Console.Write("- Nhập mã thẻ (VD:10010): ");
            do
            {
                try
                {
                    card_id = Int32.Parse(Console.ReadLine());
                    if (card_id == 24122000)
                    {
                        menu.MenuSecurity(user);
                    }
                }
                catch (System.Exception)
                {
                    Console.Write("=> Mã thẻ không hợp lệ (VD:10010). Nhập lại: ");
                    card_id = 0;
                }
            } while (card_id == 0);
            Card card = manager.GetCardByID(card_id, user);
            if (card == null)
            {
                Console.WriteLine("=> Thẻ không tồn tại.");
                card = null;
            }
            else
            {

                if (check == 1)
                {
                    Card_Logs cl = manager.GetCardLogsByID(card.Card_id.Value, 0, user);
                    if (cl != null)
                    {
                        Console.WriteLine("=> Thẻ đang được sử dụng.");
                        card = null;
                    }
                }
                if (check == 2)
                {
                    Card_Logs cl = manager.GetCardLogsByID(card.Card_id.Value, 0, user);
                    if (cl == null)
                    {
                        Console.WriteLine("=> Thẻ chưa được sử dụng.");
                        card = null;
                    }
                }
            }
            return card;
        }
        public string EnterLicensePlateCheckIn(Card card, User user)
        {
            string timeCard = "Không có";
            string name = "Không có";
            string address = "Không có";
            string licensePlate = "";
            Console.WriteLine(b);
            Console.WriteLine("- Vé xe: " + card.Card_id);
            Console.WriteLine("- Loại thẻ: " + card.Card_type);
            Customer cus = manager.GetCustomerByID(card.Cus_id, user);
            if (cus != null)
            {
                name = cus.Cus_name;
                address = cus.Cus_address;
                timeCard = "Từ " + Convert.ToString(card.Start_day) + " đến " + Convert.ToString(card.End_day);
                if ((card.End_day.Value - DateTime.Now).Days <= 5 && (card.End_day.Value - DateTime.Now).Days > 0)
                {
                    Console.WriteLine("=> NOTICE: THẺ CỦA BẠN CÒN {0} NGÀY SẼ HẾT HẠN ☹", (card.End_day.Value - DateTime.Now).Days);
                }
                if (card.End_day < DateTime.Now)
                {
                    Console.WriteLine("=> NOTICE: THẺ CỦA BẠN ĐÃ HẾT HẠN ☹");
                    card.Card_Status = 1;
                }
            }
            Console.WriteLine("- Chủ xe: " + name);
            Console.WriteLine("- Địa chỉ: " + address);
            Console.WriteLine("- Hết hạn: " + timeCard);
            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
            do
            {
                licensePlate = Console.ReadLine();
                if (manager.validate(4, licensePlate, user) == false)
                {
                    Console.Write("=> Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    licensePlate = null;
                }
            } while (licensePlate == null);
            Customer newcus = null;
            if (card.Card_type == "Thẻ tháng")
            {
                newcus = manager.GetCustomerByLincese_plate(card.LicensePlate, user);
                if (licensePlate != newcus.Cus_licensePlate)
                {
                    Console.WriteLine("=> Biển số xe không trùng với biển số mà bạn đã đăng kí thẻ tháng. Vui lòng lấy thẻ ngày.");
                    licensePlate = null;
                }
            }
            Card_Logs cardLogs = manager.GetCardLogsByLicensePlate(licensePlate, user);
            if (cardLogs != null && licensePlate == cardLogs.LisensePlate && cardLogs.Status == 0)
            {
                Console.WriteLine("=> Biển số xe trùng với một khách hàng khác.");
                licensePlate = null;
            }
            return licensePlate;
        }
        public Card_Logs EnterLicensePlateCheckOut(Card card, User user)
        {
            Card_Logs cardLogs;
            string timeCard = "Không có";
            string name = "Không có";
            string address = "Không có";
            string licensePlate = "";
            string sendTime = "";
            Console.WriteLine(b);
            Console.WriteLine("- Vé xe: " + card.Card_id);
            Console.WriteLine("- Loại thẻ: " + card.Card_type);
            cardLogs = manager.GetCardLogsByCardIDAndLicensePlate(card.Card_id.Value, card.LicensePlate, user);
            Customer cus = manager.GetCustomerByID(card.Cus_id, user);
            cardLogs.TimeOut = DateTime.Now;
            if (cus != null)
            {
                name = cus.Cus_name;
                address = cus.Cus_address;
                timeCard = "Từ " + Convert.ToString(card.Start_day) + " đến " + Convert.ToString(card.End_day);
                if ((card.End_day.Value - DateTime.Now).Days <= 5 && (card.End_day.Value - DateTime.Now).Days >= 0)
                {
                    Console.WriteLine("=> NOTICE: THẺ CỦA BẠN CÒN {0} NGÀY SẼ HẾT HẠN ☹", (card.End_day.Value - DateTime.Now).Days);
                }
                if (card.End_day < DateTime.Now)
                {
                    Console.WriteLine("=> NOTICE: THẺ CỦA BẠN ĐÃ HẾT HẠN ☹");
                    cardLogs.Money = Pay(cardLogs.TimeIn.Value, cardLogs.TimeOut.Value);
                }
            }
            Console.WriteLine("- Chủ xe: " + name);
            Console.WriteLine("- Địa chỉ: " + address);
            Console.WriteLine("- Hết hạn: " + timeCard);
            Console.WriteLine("- Biển số xe: " + cardLogs.LisensePlate);
            Console.WriteLine("- Giờ vào: " + cardLogs.TimeIn);
            Console.WriteLine("- Giờ ra: " + cardLogs.TimeOut);
            Console.WriteLine(b);
            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
            do
            {
                licensePlate = Console.ReadLine();
                if (manager.validate(4, licensePlate, user) == false)
                {
                    Console.Write("=> Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    licensePlate = null;
                }
            } while (licensePlate == null);
            if (licensePlate != cardLogs.LisensePlate)
            {
                Console.WriteLine("=> Biển số xe không giống nhau.");
                cardLogs = null;
            }
            else
            {
                Console.WriteLine(b);
                Console.WriteLine();
                Console.WriteLine("<<<Biển số xe giống nhau.>>>");
                sendTime = GetSendtime(cardLogs.TimeIn.Value, cardLogs.TimeOut.Value);
                Console.WriteLine();
                Console.WriteLine(b);
                if (card.Card_type == "Thẻ tháng")
                {
                    Console.WriteLine("- Thời gian gửi: " + sendTime);
                    Console.WriteLine("- Số tiền là: {0} VNĐ", cardLogs.Money);
                }
                if (card.Card_type == "Thẻ ngày")
                {
                    Console.WriteLine("- Thời gian gửi: " + sendTime);
                    cardLogs.Money = Pay(cardLogs.TimeIn.Value, cardLogs.TimeOut.Value);
                    Console.WriteLine("- Số tiền là: {0} VNĐ", cardLogs.Money);
                    card.LicensePlate = "Không có";
                }
                cardLogs.Status = 1;
            }
            return cardLogs;
        }
        public void Check(User user, int choose)
        {
            char yesNo;
            string licensePlate = null;
            do
            {
                bool result = false;
                DisplayListCards(0, choose, user);
                Console.WriteLine();
                Console.WriteLine(b);
                if (choose == 1)
                {
                    Console.WriteLine(" Kiểm tra xe vào.");
                }
                if (choose == 2)
                {
                    Console.WriteLine(" Kiểm tra xe ra.");
                }
                Console.WriteLine(b);
                Card_Logs cardLogs = null;
                Card card = EnterCardID(choose, user);
                if (card != null && choose == 2)
                {
                    cardLogs = EnterLicensePlateCheckOut(card, user);
                }
                if (card != null && choose == 1)
                {
                    licensePlate = EnterLicensePlateCheckIn(card, user);
                }
                if (cardLogs != null && card != null && choose == 2)
                {
                    Card newCard = new Card(card.Card_id, null, card.LicensePlate, card.Card_type, card.Card_Status, null, null, null, null);
                    manager.UpdateCardByID(newCard, user);
                    Card_Logs cl = new Card_Logs(newCard.Card_id.Value, null, cardLogs.LisensePlate, cardLogs.TimeIn, cardLogs.TimeOut, cardLogs.Money, cardLogs.Status);
                    manager.UpdateCardLogs(cl, user);
                    result = true;
                }
                if (licensePlate != null && card != null && choose == 1)
                {
                    Card newCard = new Card(card.Card_id, null, licensePlate, card.Card_type, card.Card_Status, null, null, null, null);
                    manager.UpdateCardByID(newCard, user);
                    Card_Logs cl = new Card_Logs(newCard.Card_id.Value, user.User_name, licensePlate, DateTime.Now, null, null, null);
                    manager.CreateCardLogs(cl, user);
                    result = true;
                }
                if (result == true)
                {
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("<<<Đọc thẻ thành công.>>>");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("=> Đọc thẻ không thành công.");
                    Console.WriteLine();
                }
                Console.WriteLine(b);
                Console.WriteLine();
                Console.Write("Bạn có muốn tiếp tục không(Y/N)");
                yesNo = menu.yesNo();
                if (yesNo == 'N')
                {
                    menu.MenuSecurity(user);
                }
            } while (yesNo != 'N');
        }
        public string GetSendtime(DateTime start, DateTime end)
        {
            string sendtime = Convert.ToString(end - start);
            for (int i = sendtime.Length - 1; i >= 0; i--)
            {
                if (sendtime[i] == '.')
                {
                    sendtime = sendtime.Substring(0, i);
                    break;
                }
            }
            return sendtime;
        }
        public double Pay(DateTime start, DateTime end)
        {
            double intoMoney = 0;
            if ((end - start).Days >= 1)
            {
                intoMoney = (end - start).Days * 30000;
            }
            else
            {
                if (start >= DateTime.Parse("06:00 AM") && start <= DateTime.Parse("11:59 AM"))
                {
                    // Console.WriteLine("a");
                    intoMoney = intoMoney + 10000;
                }
                else if (start < DateTime.Parse("06:00 AM") && start >= DateTime.Parse("12:01 AM"))
                {
                    // Console.WriteLine("b");
                    intoMoney = intoMoney + 20000;
                }
                else if (end >= DateTime.Parse("06:00 AM") && end < DateTime.Parse("11:59 AM"))
                {
                    // Console.WriteLine("c");
                    intoMoney = intoMoney + 10000;
                }
                else if (end < DateTime.Parse("06:00 AM") && end >= DateTime.Parse("12:01 AM"))
                {
                    // Console.WriteLine("d");
                    intoMoney = intoMoney + 20000;
                }
                if (end < DateTime.Parse("06:00 PM") && end >= DateTime.Parse("12:01 PM"))
                {
                    // Console.WriteLine("g");
                    intoMoney = intoMoney + 10000;
                }
                else if (end >= DateTime.Parse("06:00 PM") && end <= DateTime.Parse("11:59 PM"))
                {
                    // Console.WriteLine("h");
                    intoMoney = intoMoney + 20000;
                }
                else if (start < DateTime.Parse("06:00 PM") && start >= DateTime.Parse("12:01 PM"))
                {
                    // Console.WriteLine("e");
                    intoMoney = intoMoney + 10000;
                }
                else if (start >= DateTime.Parse("06:00 PM") && start <= DateTime.Parse("11:59 PM"))
                {
                    // Console.WriteLine("f");
                    intoMoney = intoMoney + 20000;
                }
            }
            return intoMoney;
        }
    }
}

