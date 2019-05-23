using System;
using System.Collections.Generic;
using BL;
using Persistence;
using PL_console;
namespace PL_Console
{
    public class SecurityCheckInCheckOut
    {
        private Menus menu = new Menus();
        string b = "══════════════════════════════════════════════════════════════";
        private ManagerCardAndStatistic manager = new ManagerCardAndStatistic();
        private CardBL cardBL = new CardBL();
        private Card_LogsBL cardLogsBL = new Card_LogsBL();
        private Card_Logs cardLogs = null;
        private List<Card> listcard = null;
        private Card_Detail cardDetail = null;
        private Customer cus = null;
        private Card card = null;
        bool checkCardBL = false;
        bool checkCardLogsBL = false;
        char yesNo;
        public void CheckIn(User user)
        {
            do
            {
                listcard = manager.GetlistCard();
                if (listcard.Capacity == 0)
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
                    string card_id;
                    string licensePlate = "";
                    string status = "";
                    string dateTimeStart = "";
                    var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Loại thẻ", "Trạng thái", "Ngày giờ xe vào");
                    foreach (var item in listcard)
                    {
                        cardLogs = manager.GetCardLogsByLisencePlateAndCardID(item.LicensePlate, item.Card_id);
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
                    Console.WriteLine(b);
                    Console.WriteLine(" Kiểm tra xe vào.");
                    Console.WriteLine(b);
                    Console.Write("- Nhập mã thẻ (VD:CM01): ");
                    do
                    {
                        card_id = manager.validate(1);
                        card = manager.GetCardByID(card_id);
                        if (card == null)
                        {
                            Console.WriteLine("↻ Thẻ không tồn tại.");
                            break;
                        }
                        if (card.Card_Status == 1)
                        {
                            Console.WriteLine("↻ Thẻ đang được sử dụng.");
                            break;
                        }
                    } while (card == null || card.Card_Status == 1);
                    if (card != null && card.Card_Status == 0)
                    {

                        Console.WriteLine(b);
                        Console.WriteLine("- Vé xe: " + card_id);
                        Console.WriteLine("- Loại thẻ: " + card.Card_type);
                        if (card.Card_type == "Thẻ ngày")
                        {
                            Card newcard = null;
                            Console.WriteLine("- Chủ xe: Không có");
                            Console.WriteLine("- Địa chỉ: Không có");
                            Console.WriteLine("- Hết hạn: Không có");
                            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
                            licensePlate = manager.validate(5);
                            newcard = manager.GetCardByLicensePlate(licensePlate);
                            if (newcard != null)
                            {
                                Console.WriteLine("↻ Biển số xe trùng với một khách hàng khác.");
                                licensePlate = null;
                            }
                            card.Card_Status = 1;
                        }
                        if (card.Card_type == "Thẻ tháng")
                        {
                            cus = manager.GetCustomerByLincese_plate(card.LicensePlate);
                            cardDetail = manager.GetCard_DetailbyID(card.Card_id);
                            Console.WriteLine("- Chủ xe: " + cus.Cus_name);
                            Console.WriteLine("- Địa chỉ: " + cus.Cus_address);
                            Console.WriteLine("- Hết hạn: " + cardDetail.End_day);
                            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
                            licensePlate = manager.validate(5);
                            if (licensePlate != cus.Cus_licensePlate)
                            {
                                Console.WriteLine("↻ Biển số xe không trùng với biển số mà bạn đã đăng kí thẻ tháng. Vui lòng lấy thẻ ngày.");
                                licensePlate = null;
                            }
                            card.Card_Status = 1;
                        }
                        if (licensePlate != null)
                        {
                            try
                            {
                                cardBL = new CardBL();
                                checkCardBL = cardBL.UpdateCardByID(new Card(null, licensePlate, null, 1, null, null), card_id);
                                cardLogsBL = new Card_LogsBL();
                                checkCardLogsBL = cardLogsBL.CreateCardLogs(new Card_Logs(card_id, user.User_name, licensePlate, DateTime.Now, null, null, null));
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
                            if (checkCardBL == true && checkCardLogsBL == true)
                            {
                                Console.WriteLine(b);
                                Console.WriteLine();
                                Console.WriteLine("✔ Đọc thẻ thành công.");
                                Console.WriteLine();
                            }
                            else if (checkCardBL == false && checkCardLogsBL == false)
                            {
                                Console.WriteLine(b);
                                Console.WriteLine();
                                Console.WriteLine("✘ Đọc thẻ không thành công.");
                                Console.WriteLine();
                            }
                        }
                    }
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
        public void ChecOut(User user)
        {
            do
            {
                listcard = manager.GetlistCard();
                if (listcard.Capacity == 0)
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
                    string licensePlate = "";
                    int count = 0;
                    string card_id;
                    string status = "";
                    string dateTimeBikeIn = "";
                    string sendtime = "";
                    double intoMoney = 0;
                    var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Loại thẻ", "Trạng thái", "Ngày giờ xe vào");
                    foreach (var item in listcard)
                    {
                        cardLogs = manager.GetCardLogsByLisencePlateAndCardID(item.LicensePlate, item.Card_id);
                        if (item.Card_Status == 1)
                        {
                            status = "Hoạt động";
                            if (cardLogs != null)
                            {
                                dateTimeBikeIn = Convert.ToString(cardLogs.DateTimeStart);
                            }
                            table.AddRow(item.Card_id, item.LicensePlate, item.Card_type, status, dateTimeBikeIn);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Không có xe trong bãi!!! Nhấn Enter để quay lại.");
                        Console.ReadKey();
                        menu.CheckInCheckOut(user);
                    }
                    table.Write(Format.Alternative);
                    Console.Write("Nhấn Enter để tiếp tục");
                    Console.ReadKey();
                    Console.WriteLine();
                    Console.WriteLine(b);
                    Console.WriteLine(" Kiểm tra xe ra.");
                    Console.WriteLine(b);
                    Console.Write("- Nhập mã thẻ (VD:CM01): ");
                    do
                    {
                        card_id = manager.validate(1);
                        card = manager.GetCardByID(card_id);
                        if (card == null)
                        {
                            Console.WriteLine("↻ Mã thẻ không tồn tại.");
                            break;
                        }
                        if (card.Card_Status == 0)
                        {
                            Console.WriteLine("↻ Thẻ chưa được sử dụng.");
                            break;
                        }
                    } while (card == null || card.Card_Status == 0);
                    if (card != null && card.Card_Status != 0)
                    {
                        Console.WriteLine(b);
                        Console.WriteLine("- Vé xe: " + card.Card_id);
                        Console.WriteLine("- Loại thẻ: " + card.Card_type);
                        DateTime dateTimeEnd = DateTime.Now;
                        if (card.Card_type == "Thẻ tháng")
                        {
                            cus = manager.GetCustomerByLincese_plate(card.LicensePlate);
                            cardDetail = manager.GetCard_DetailbyID(card_id);
                            cardLogs = manager.GetCardLogsByLisencePlateAndCardID(card.LicensePlate, card_id);
                            Console.WriteLine("- Chủ xe: " + cus.Cus_name);
                            Console.WriteLine("- Địa chỉ: " + cus.Cus_address);
                            Console.WriteLine("- Hết hạn: " + cardDetail.End_day);
                            Console.WriteLine("- Biển số xe: " + cardLogs.LisensePlate);
                            Console.WriteLine("- Giờ vào: " + cardLogs.DateTimeStart);
                            Console.WriteLine("- Giờ ra: " + dateTimeEnd);
                            Console.WriteLine(b);
                            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
                            do
                            {
                                licensePlate = manager.validate(5);
                                if (licensePlate != cardLogs.LisensePlate)
                                {
                                    Console.WriteLine("↻ Biển số xe không giống nhau.");
                                    licensePlate = null;
                                    break;
                                }
                                if (licensePlate == cardLogs.LisensePlate)
                                {

                                    Console.WriteLine(b);
                                    Console.WriteLine();
                                    Console.WriteLine("✔ Biển số xe giống nhau.");
                                    sendtime = GetSendtime(cardLogs.DateTimeStart.Value, dateTimeEnd);

                                    Console.WriteLine();
                                    Console.WriteLine(b);
                                    Console.WriteLine("- Thời gian gửi: " + sendtime);
                                    if (cardDetail.End_day >= DateTime.Now)
                                    {
                                        intoMoney = Pay(cardLogs.DateTimeStart.Value, dateTimeEnd);
                                        Console.WriteLine("- Thẻ của bạn đã quá hạn sử dụng.");
                                    }
                                    Console.WriteLine("- Số tiền là: {0} VNĐ", intoMoney);
                                }
                            } while (licensePlate == null);
                        }
                        if (card.Card_type == "Thẻ ngày")
                        {
                            cardLogs = manager.GetCardLogsByLisencePlateAndCardID(card.LicensePlate, card_id);
                            Console.WriteLine("- Chủ xe: Không có");
                            Console.WriteLine("- Địa chỉ: Không có");
                            Console.WriteLine("- Hết hạn: Không có");
                            Console.WriteLine("- Biển số xe: " + cardLogs.LisensePlate);
                            Console.WriteLine("- Giờ vào: " + cardLogs.DateTimeStart);
                            Console.WriteLine("- Giờ ra: " + dateTimeEnd);
                            Console.WriteLine(b);
                            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
                            do
                            {
                                licensePlate = manager.validate(5);
                                if (licensePlate != cardLogs.LisensePlate)
                                {
                                    Console.WriteLine("↻ Biển số xe không giống nhau.");
                                    licensePlate = null;
                                    break;
                                }
                                if (licensePlate == cardLogs.LisensePlate)
                                {
                                    Console.WriteLine(b);
                                    Console.WriteLine();
                                    Console.WriteLine("✔ Biển số xe giống nhau.");
                                    sendtime = GetSendtime(cardLogs.DateTimeStart.Value, dateTimeEnd);
                                    Console.WriteLine();
                                    Console.WriteLine(b);
                                    Console.WriteLine("- Thời gian gửi: " + sendtime);
                                    intoMoney = Pay(cardLogs.DateTimeStart.Value, dateTimeEnd);
                                    Console.WriteLine("- Số tiền là: {0} VNĐ", intoMoney);
                                    licensePlate = "No License Plate";
                                }
                            } while (licensePlate == null);
                        }
                        if (licensePlate != null)
                        {
                            try
                            {
                                cardBL = new CardBL();
                                checkCardBL = cardBL.UpdateCardByID(new Card(null, licensePlate, null, 0, null, null), card_id);
                                cardLogsBL = new Card_LogsBL();
                                checkCardLogsBL = cardLogsBL.UpdateCardLogsByLicensePlateAndCardID(new Card_Logs(null, null, null, null, dateTimeEnd, sendtime, intoMoney), licensePlate, card_id, cardLogs.DateTimeStart?.ToString("yyyy/MM/dd HH:mm:ss"));
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
                            if (checkCardBL == true && checkCardLogsBL == true)
                            {
                                Console.WriteLine(b);
                                Console.WriteLine();
                                Console.WriteLine("✔ Đọc thẻ thành công.");
                                Console.WriteLine();
                            }
                            else if (checkCardBL == false && checkCardLogsBL == false)
                            {
                                Console.WriteLine(b);
                                Console.WriteLine();
                                Console.WriteLine("✘ Đọc thẻ không thành công.");
                                Console.WriteLine();
                            }
                        }
                    }
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