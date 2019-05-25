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
            List<Card> ListCards = manager.GetListCards();
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
                foreach (var item in ListCards)
                {
                    Card_Logs cardLogs = manager.GetCardLogsByLisencePlateAndCardID(item.LicensePlate, item.Card_id);
                    if (item.Card_Status == 0)
                    {
                        status = "Không hoạt động";
                        if (cardLogs == null)
                        {
                            dateTimeStart = "Không có";
                        }
                    }
                    else if (item.Card_Status == 1)
                    {
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
        public Card EnterCardID()
        {
            string card_id;
            Console.Write("- Nhập mã thẻ (VD:CM01): ");
            do
            {
                card_id = Console.ReadLine();
                if (manager.validate(1, card_id) == false)
                {
                    Console.Write("↻ Mã thẻ không hợp lệ (VD:CM01). Nhập lại: ");
                    card_id = null;
                }
            } while (card_id == null);
            Card card = manager.GetCardByID(card_id);
            if (card == null)
            {
                Console.WriteLine("↻ Thẻ không tồn tại.");
                card = null;
            }
            return card;
        }
        public string EnterLicensePlateCheckIn(Card card)
        {
            string licensePlate = "";
            Console.WriteLine(b);
            Console.WriteLine("- Vé xe: " + card.Card_id);
            Console.WriteLine("- Loại thẻ: " + card.Card_type);
            if (card.Card_type == "Thẻ ngày")
            {
                Card newcard = null;
                Console.WriteLine("- Chủ xe: Không có");
                Console.WriteLine("- Địa chỉ: Không có");
                Console.WriteLine("- Hết hạn: Không có");
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
                newcard = manager.GetCardByLicensePlate(licensePlate);
                if (newcard != null)
                {
                    Console.WriteLine("↻ Biển số xe trùng với một khách hàng khác.");
                    licensePlate = null;
                }
            }
            if (card.Card_type == "Thẻ tháng")
            {
                Customer cus = manager.GetCustomerByLincese_plate(card.LicensePlate);
                Card_Detail cardDetail = manager.GetCardDetailByID(card.Card_id);
                Console.WriteLine("- Chủ xe: " + cus.Cus_name);
                Console.WriteLine("- Địa chỉ: " + cus.Cus_address);
                Console.WriteLine("- Hết hạn: " + cardDetail.End_day);
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
                if (licensePlate != cus.Cus_licensePlate)
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
                Card card = EnterCardID();
                if (card != null)
                {
                    if (card.Card_Status == 1)
                    {
                        Console.WriteLine("↻ Thẻ đang được sử dụng.");
                        card = null;
                    }
                    else
                    {
                        licensePlate = EnterLicensePlateCheckIn(card);
                    }
                }
                if (licensePlate != null && card != null)
                {
                    Card newCard = new Card(card.Card_id, licensePlate, null, card.Card_Status, null, null, null);
                    Card_Logs cl = new Card_Logs(newCard.Card_id, user.User_name, licensePlate, DateTime.Now, null, null, null);
                    manager.UpdateCardByID(newCard);
                    manager.CreateCardLogs(cl);
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
        public Card_Logs EnterLicensePlateCheckOut(Card card)
        {
            Card_Logs cardLogs;
            string licensePlate = "";
            Console.WriteLine(b);
            Console.WriteLine("- Vé xe: " + card.Card_id);
            Console.WriteLine("- Loại thẻ: " + card.Card_type);
            cardLogs = manager.GetCardLogsByLisencePlateAndCardID(card.LicensePlate, card.Card_id);
            cardLogs.IntoMoney = 0;
            cardLogs.DateTimeEnd = DateTime.Now;
            if (card.Card_type == "Thẻ tháng")
            {
                Customer cus = manager.GetCustomerByLincese_plate(card.LicensePlate);
                Card_Detail cardDetail = manager.GetCardDetailByID(card.Card_id);
                Console.WriteLine("- Chủ xe: " + cus.Cus_name);
                Console.WriteLine("- Địa chỉ: " + cus.Cus_address);
                Console.WriteLine("- Hết hạn: " + cardDetail.End_day);
                Console.WriteLine("- Biển số xe: " + cardLogs.LisensePlate);
                Console.WriteLine("- Giờ vào: " + cardLogs.DateTimeStart);
                Console.WriteLine("- Giờ ra: " + cardLogs.DateTimeEnd);
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
                    Console.WriteLine("- Thời gian gửi: " + cardLogs.SendTime);
                    if (cardDetail.End_day <= DateTime.Now)
                    {
                        cardLogs.IntoMoney = Pay(cardLogs.DateTimeStart.Value, cardLogs.DateTimeEnd.Value);
                        Console.WriteLine("- Thẻ của bạn đã quá hạn sử dụng.");
                    }
                    Console.WriteLine("- Số tiền là: {0} VNĐ", cardLogs.IntoMoney);
                }
            }
            if (card.Card_type == "Thẻ ngày")
            {
                Console.WriteLine("- Chủ xe: Không có");
                Console.WriteLine("- Địa chỉ: Không có");
                Console.WriteLine("- Hết hạn: Không có");
                Console.WriteLine("- Biển số xe: " + cardLogs.LisensePlate);
                Console.WriteLine("- Giờ vào: " + cardLogs.DateTimeStart);
                Console.WriteLine("- Giờ ra: " + cardLogs.DateTimeEnd);
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
                Card card = EnterCardID();
                if (card != null)
                {
                    if (card.Card_Status == 0)
                    {
                        Console.WriteLine("↻ Thẻ chưa được sử dụng.");
                        card = null;
                    }
                    else
                    {
                        cardLogs = EnterLicensePlateCheckOut(card);
                    }
                }
                if (cardLogs != null && card != null)
                {
                    Card newCard = new Card(card.Card_id, card.LicensePlate, null, card.Card_Status, null, null, null);
                    Card_Logs cl = new Card_Logs(newCard.Card_id, null, cardLogs.LisensePlate, cardLogs.DateTimeStart, cardLogs.DateTimeEnd, cardLogs.SendTime, cardLogs.IntoMoney);
                    manager.UpdateCardByID(newCard);
                    manager.UpdateCardLogs(cl);
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
            if (start <= DateTime.Parse("06:00:00 PM") && start >= DateTime.Parse("06:00:00 AM"))
            {
                intoMoney = intoMoney + 10000;
            }
            else if (start <= DateTime.Parse("06:00:00 AM") && start >= DateTime.Parse("06:00:00 PM"))
            {
                intoMoney = intoMoney + 20000;
            }
            else if (end >= DateTime.Parse("06:00:00 AM") && end <= DateTime.Parse("06:00:00 PM"))
            {
                intoMoney = intoMoney + 10000;
            }
            else if (end <= DateTime.Parse("06:00:00 AM") && end >= DateTime.Parse("06:00:00 pM"))
            {
                intoMoney = intoMoney + 20000;
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
