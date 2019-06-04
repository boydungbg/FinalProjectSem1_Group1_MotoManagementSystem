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
        public void DisplayListCards(User user)
        {
            List<Card> ListCards = manager.GetListCards(user);
            if (ListCards.Capacity == 0)
            {
                Console.Clear();
                Console.WriteLine("Không có dữ liệu của thẻ!!! Nhấn Enter để quay lại.");
                Console.ReadKey();
                menu.CheckInCheckOut(user);
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                string status = "";
                string dateTimeStart = "";
                var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Loại thẻ", "Trạng thái", "Ngày giờ xe vào");
                Card_Logs cardLogs = null;
                foreach (var item in ListCards)
                {
                    if (item.Card_Status == 0)
                    {
                        status = "Không hoạt động";
                        dateTimeStart = "Không có";
                    }
                    if (item.Card_Status == 1)
                    {
                        cardLogs = manager.GetCardLogsByCardIDAndLicensePlate(item.Card_id.Value, item.LicensePlate, user);
                        status = "Hoạt động";
                        if (cardLogs != null)
                        {
                            dateTimeStart = Convert.ToString(cardLogs.DateTimeStart);
                        }
                    }
                    table.AddRow(item.Card_id, item.LicensePlate, item.Card_type, status, dateTimeStart);
                }
                table.Write(Format.Alternative);
                Console.Write("Nhấn Enter để tiếp tục");
                Console.ReadKey();
                Console.WriteLine();
            }
        }
        public Card EnterCardID(User user)
        {
            int card_id;
            Console.Write("- Nhập mã thẻ (VD:10010): ");
            do
            {
                try
                {
                    card_id = Int32.Parse(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    Console.Write("↻ Mã thẻ không hợp lệ (VD:10010). Nhập lại: ");
                    card_id = 0;
                }
            } while (card_id == 0);
            Card card = manager.GetCardByID(card_id, user);
            if (card == null)
            {
                Console.WriteLine("↻ Thẻ không tồn tại.");
                card = null;
            }
            return card;
        }
        public string EnterLicensePlateCheckIn(Card card, User user)
        {
            string licensePlate = "";
            string timeCard = "";
            string name = "";
            string address = "";
            Console.WriteLine(b);
            Console.WriteLine("- Vé xe: " + card.Card_id);
            Console.WriteLine("- Loại thẻ: " + card.Card_type);
            Card_Detail cardDetail = manager.GetCardDetailByID(card.Card_id, user);
            if (cardDetail == null)
            {
                name = "Không có";
                address = "Không có";
                timeCard = "Không có";
            }
            if (cardDetail != null)
            {
                Customer cus = manager.GetCustomerByID(cardDetail.Cus_id, user);
                name = cus.Cus_name;
                address = cus.Cus_address;
                timeCard = "Từ " + Convert.ToString(cardDetail.Start_day) + " đến " + Convert.ToString(cardDetail.End_day);
            }
            Console.WriteLine("- Chủ xe: " + name);
            Console.WriteLine("- Địa chỉ: " + address);
            Console.WriteLine("- Hết hạn: " + timeCard);
            if (cardDetail != null)
            {
                if ((cardDetail.End_day.Value - DateTime.Now).Days <= 5 && (cardDetail.End_day.Value - DateTime.Now).Days >= 0)
                {
                    Console.WriteLine("↪ NOTICE: THẺ CỦA BẠN CÒN {0} NGÀY SẼ HẾT HẠN ☹", (cardDetail.End_day.Value - DateTime.Now).Days);
                }
                if (cardDetail.End_day < DateTime.Now)
                {
                    Console.WriteLine("↪ NOTICE: THẺ CỦA BẠN ĐÃ HẾT HẠN ☹");
                    card.Card_type = "Thẻ ngày";
                    manager.UpdateCardByID(new Card(card.Card_id, "No License Plate", card.Card_type, 0, null, null, null), user);
                }
            }
            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
            do
            {
                licensePlate = Console.ReadLine();
                if (manager.validate(4, licensePlate) == false)
                {
                    Console.Write("↻ Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    licensePlate = null;
                }
            } while (licensePlate == null);
            if (card.Card_type == "Thẻ ngày")
            {
                Card newcard = null;
                newcard = manager.GetCardByLicensePlate(licensePlate, user);
                if (newcard != null)
                {
                    Console.WriteLine("↻ Biển số xe trùng với một khách hàng khác.");
                    licensePlate = null;
                }
            }
            if (card.Card_type == "Thẻ tháng")
            {
                Customer newcus = null;
                newcus = manager.GetCustomerByLincese_plate(card.LicensePlate, user);
                if (licensePlate != newcus.Cus_licensePlate)
                {
                    Console.WriteLine("↻ Biển số xe không trùng với biển số mà bạn đã đăng kí thẻ tháng. Vui lòng lấy thẻ ngày.");
                    licensePlate = null;
                }
            }
            card.Card_Status = 1;
            return licensePlate;
        }
        public void CheckIn(User user)
        {
            char yesNo;
            string licensePlate = null;
            do
            {
                Console.Clear();
                DisplayListCards(user);
                Console.WriteLine();
                Console.WriteLine(b);
                Console.WriteLine(" Kiểm tra xe vào.");
                Console.WriteLine(b);
                Card card = EnterCardID(user);
                if (card != null)
                {
                    if (card.Card_Status == 1)
                    {
                        Console.WriteLine("↻ Thẻ đang được sử dụng.");
                        card = null;
                    }
                    else
                    {
                        licensePlate = EnterLicensePlateCheckIn(card, user);
                    }
                }
                if (licensePlate != null && card != null)
                {
                    Card newCard = new Card(card.Card_id, licensePlate, card.Card_type, card.Card_Status, null, null, null);
                    Card_Logs cl = new Card_Logs(newCard.Card_id.Value, user.User_name, licensePlate, DateTime.Now, null, null, null);
                    manager.UpdateCardByID(newCard, user);
                    manager.CreateCardLogs(cl, user);
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("✔ Đọc thẻ thành công.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("✘ Đọc thẻ không thành công.");
                    Console.WriteLine();
                }
                Console.WriteLine(b);
                Console.WriteLine();
                Console.Write("Bạn có muốn tiếp tục không(Y/N)");
                yesNo = menu.yesNo();
                if (yesNo == 'N')
                {
                    menu.CheckInCheckOut(user);
                }
            } while (yesNo != 'N');
        }
        public Card_Logs EnterLicensePlateCheckOut(Card card, User user)
        {
            Card_Logs cardLogs;
            string timeCard = "";
            string licensePlate = "";
            string name = "";
            string address = "";
            Console.WriteLine(b);
            Console.WriteLine("- Vé xe: " + card.Card_id);
            Console.WriteLine("- Loại thẻ: " + card.Card_type);
            cardLogs = manager.GetCardLogsByCardIDAndLicensePlate(card.Card_id.Value, card.LicensePlate, user);
            Card_Detail cardDetail = manager.GetCardDetailByID(card.Card_id, user);
            cardLogs.IntoMoney = 0;
            cardLogs.DateTimeEnd = DateTime.Now;
            if (cardDetail == null)
            {
                name = "Không có";
                address = "Không có";
                timeCard = "Không có";
            }
            if (cardDetail != null)
            {
                Customer cus = manager.GetCustomerByID(cardDetail.Cus_id, user);
                name = cus.Cus_name;
                address = cus.Cus_address;
                timeCard = "Từ " + Convert.ToString(cardDetail.Start_day) + " đến " + Convert.ToString(cardDetail.End_day);
            }
            Console.WriteLine("- Chủ xe: " + name);
            Console.WriteLine("- Địa chỉ: " + address);
            Console.WriteLine("- Hết hạn: " + timeCard);
            Console.WriteLine("- Biển số xe: " + cardLogs.LisensePlate);
            Console.WriteLine("- Giờ vào: " + cardLogs.DateTimeStart);
            Console.WriteLine("- Giờ ra: " + cardLogs.DateTimeEnd);
            if (cardDetail != null)
            {
                if ((cardDetail.End_day.Value - DateTime.Now).Days <= 5 && (cardDetail.End_day.Value - DateTime.Now).Days >= 0)
                {
                    Console.WriteLine("↪ NOTICE: THẺ CỦA BẠN CÒN {0} NGÀY SẼ HẾT HẠN ☹", (cardDetail.End_day.Value - DateTime.Now).Days);
                }
                if (cardDetail.End_day < DateTime.Now)
                {
                    Console.WriteLine("↪ NOTICE: THẺ CỦA BẠN ĐÃ HẾT HẠN ☹");
                }
            }
            Console.WriteLine(b);
            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
            do
            {
                licensePlate = Console.ReadLine();
                if (manager.validate(4, licensePlate) == false)
                {
                    Console.Write("↻ Biển số xe không hợp lệ (VD:88X8-8888). Nhập lại: ");
                    licensePlate = null;
                }
            } while (licensePlate == null);
            if (licensePlate != cardLogs.LisensePlate)
            {
                Console.WriteLine("↻ Biển số xe không giống nhau.");
                cardLogs = null;
            }
            else
            {
                Console.WriteLine(b);
                Console.WriteLine();
                Console.WriteLine("✔ Biển số xe giống nhau.");
                cardLogs.SendTime = GetSendtime(cardLogs.DateTimeStart.Value, cardLogs.DateTimeEnd.Value);
                Console.WriteLine();
                Console.WriteLine(b);
                if (card.Card_type == "Thẻ tháng")
                {
                    Console.WriteLine("- Thời gian gửi: " + cardLogs.SendTime);
                    Console.WriteLine("- Số tiền là: {0} VNĐ", cardLogs.IntoMoney);
                }
                if (card.Card_type == "Thẻ ngày")
                {
                    Console.WriteLine("- Thời gian gửi: " + cardLogs.SendTime);
                    cardLogs.IntoMoney = Pay(cardLogs.DateTimeStart.Value, cardLogs.DateTimeEnd.Value);
                    Console.WriteLine("- Số tiền là: {0} VNĐ", cardLogs.IntoMoney);
                    card.LicensePlate = "No License Plate";
                }
            }
            card.Card_Status = 0;
            return cardLogs;
        }
        public void CheckOut(User user)
        {
            char yesNo;
            do
            {
                Card_Logs cardLogs = null;
                DisplayListCards(user);
                Console.WriteLine();
                Console.WriteLine(b);
                Console.WriteLine(" Kiểm tra xe ra.");
                Console.WriteLine(b);
                Card card = EnterCardID(user);
                if (card != null)
                {
                    if (card.Card_Status == 0)
                    {
                        Console.WriteLine("↻ Thẻ chưa được sử dụng.");
                        card = null;
                    }
                    else
                    {
                        cardLogs = EnterLicensePlateCheckOut(card, user);
                    }
                }
                if (cardLogs != null && card != null)
                {
                    Card newCard = new Card(card.Card_id, card.LicensePlate, card.Card_type, card.Card_Status, null, null, null);
                    Card_Logs cl = new Card_Logs(newCard.Card_id.Value, null, cardLogs.LisensePlate, cardLogs.DateTimeStart, cardLogs.DateTimeEnd, cardLogs.SendTime, cardLogs.IntoMoney);
                    manager.UpdateCardByID(newCard, user);
                    manager.UpdateCardLogs(cl, user);
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("✔ Đọc thẻ thành công.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(b);
                    Console.WriteLine();
                    Console.WriteLine("✘ Đọc thẻ không thành công.");
                    Console.WriteLine();
                }
                Console.WriteLine(b);
                Console.WriteLine();
                Console.Write("Bạn có muốn tiếp tục không(Y/N)");
                yesNo = menu.yesNo();
                if (yesNo == 'N')
                {
                    menu.CheckInCheckOut(user);
                }
            } while (yesNo != 'N');
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
                if (start < DateTime.Parse("06:00 PM") && start >= DateTime.Parse("12:01 PM"))
                {
                    // Console.WriteLine("e");
                    intoMoney = intoMoney + 10000;
                }
                else if (start >= DateTime.Parse("06:00 PM") && start <= DateTime.Parse("11:59 PM"))
                {
                    // Console.WriteLine("f");
                    intoMoney = intoMoney + 20000;
                }
                else if (end < DateTime.Parse("06:00 PM") && end >= DateTime.Parse("12:01 PM"))
                {
                    // Console.WriteLine("g");
                    intoMoney = intoMoney + 10000;
                }
                else if (end >= DateTime.Parse("06:00 PM") && end <= DateTime.Parse("11:59 PM"))
                {
                    // Console.WriteLine("h");
                    intoMoney = intoMoney + 20000;
                }
            }
            return intoMoney;
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
    }
}
