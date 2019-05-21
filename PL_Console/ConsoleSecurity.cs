using System;
using System.Collections.Generic;
using BL;
using Persistence;
using PL_console;


namespace PL_Console
{
    public class ConsoleSecurity
    {
        private Menus menu = new Menus();
        string b = "══════════════════════════════════════════════════════════════";
        Card card = new Card();
        ConsoleManager manager = new ConsoleManager();
        Card_Logs cardLogs = new Card_Logs();
        Customer cus = new Customer();
        CustomerBL cusBL = new CustomerBL();
        Card_Detail cardDetail = new Card_Detail();
        Card_detailBL cardDetailBL = new Card_detailBL();
        List<Card> listcard = null;
        CardBL cardBL = new CardBL();
        Card_LogsBL cardLogsBL = new Card_LogsBL();
        bool checkCardBL = false;
        bool checkCardLogsBL = false;
        char yesNo;
        public void CheckIn(User user)
        {
            do
            {
                try
                {
                    listcard = cardBL.GetlistCard();
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
                        try
                        {
                            cardLogs = cardLogsBL.GetCardLogsByLisencePlateAndCardID(item.LicensePlate, item.Card_id);
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
                        try
                        {
                            cardBL = new CardBL();
                            card = cardBL.GetCardByID(card_id);
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
                            Console.WriteLine("- Chủ xe: Không có");
                            Console.WriteLine("- Địa chỉ: Không có");
                            Console.WriteLine("- Hết hạn: Không có");
                            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
                            licensePlate = manager.validate(5);
                            Card newcard = null;
                            try
                            {
                                cardLogsBL = new Card_LogsBL();
                                cardLogs = cardLogsBL.GetCardLogsByLisencePlateAndCardID(licensePlate, card_id);
                                cardBL = new CardBL();
                                newcard = cardBL.GetCardByLicensePlate(licensePlate);
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
                            if (cardLogs != null)
                            {
                                Console.WriteLine("↻ Biển số xe đã có trong bãi.");
                                licensePlate = null;
                            }
                            else if (newcard != null)
                            {
                                Console.WriteLine("↻ Biển số xe đã tồn tại trong một thẻ khác.");
                                licensePlate = null;
                            }
                        }
                        if (card.Card_type == "Thẻ tháng")
                        {
                            try
                            {
                                cusBL = new CustomerBL();
                                cus = cusBL.GetCustomerByLincese_plate(card.LicensePlate);
                                cardDetailBL = new Card_detailBL();
                                cardDetail = cardDetailBL.GetCard_DetailbyID(card.Card_id);
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
                        }
                        if (licensePlate != null)
                        {
                            // Console.WriteLine(card_id);
                            // Console.WriteLine(user.User_name);
                            // Console.WriteLine(licensePlate);
                            // Console.WriteLine(DateTime.Now);
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
                try
                {
                    listcard = cardBL.GetlistCard();
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
                    string dateTimeStart = "";
                    string sendtime = "";
                    double intoMoney = 0;
                    var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Loại thẻ", "Trạng thái", "Ngày giờ xe vào");
                    foreach (var item in listcard)
                    {
                        try
                        {
                            cardLogs = cardLogsBL.GetCardLogsByLisencePlateAndCardID(item.LicensePlate, item.Card_id);
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
                        if (item.Card_Status == 1)
                        {
                            status = "Hoạt động";
                            if (cardLogs != null)
                            {
                                dateTimeStart = Convert.ToString(cardLogs.DateTimeStart);
                            }
                            table.AddRow(item.Card_id, item.LicensePlate, item.Card_type, status, dateTimeStart);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Không có xe nào gửi!!! Nhấn Enter để quay lại.");
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
                        try
                        {
                            cardBL = new CardBL();
                            card = cardBL.GetCardByID(card_id);
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
                            try
                            {
                                cusBL = new CustomerBL();
                                cus = cusBL.GetCustomerByLincese_plate(card.LicensePlate);
                                cardDetailBL = new Card_detailBL();
                                cardDetail = cardDetailBL.GetCard_DetailbyID(card.Card_id);
                                cardLogsBL = new Card_LogsBL();
                                cardLogs = cardLogsBL.GetCardLogsByLisencePlateAndCardID(card.LicensePlate, card_id);
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
                                    sendtime = Convert.ToString(dateTimeEnd - cardLogs.DateTimeStart);
                                    Console.WriteLine();
                                    Console.WriteLine(b);
                                    Console.WriteLine("- Thời gian gửi: " + sendtime);
                                    Console.WriteLine("- Số tiền là: {0} VNĐ", intoMoney);
                                }
                            } while (licensePlate == null);
                        }
                        if (card.Card_type == "Thẻ ngày")
                        {
                            try
                            {
                                cardLogsBL = new Card_LogsBL();
                                cardLogs = cardLogsBL.GetCardLogsByLisencePlateAndCardID(card.LicensePlate, card_id);
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
                                    sendtime = Convert.ToString(dateTimeEnd - cardLogs.DateTimeStart);
                                    Console.WriteLine();
                                    Console.WriteLine(b);
                                    Console.WriteLine("- Thời gian gửi: " + sendtime);
                                    if (dateTimeEnd >= DateTime.Parse("6:00 AM"))
                                    {
                                        intoMoney = intoMoney + 10000;
                                    }
                                    else if (dateTimeEnd < DateTime.Parse("6:00 PM"))
                                    {
                                        intoMoney = intoMoney + 10000;
                                    }
                                    else if (dateTimeEnd >= DateTime.Parse("6:00 PM"))
                                    {
                                        intoMoney = intoMoney + 20000;
                                    }
                                    else if (dateTimeEnd < DateTime.Parse("6:00 AM"))
                                    {
                                        intoMoney = intoMoney + 20000;
                                    }
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
    }
}