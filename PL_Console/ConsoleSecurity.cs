using System;
using System.Collections.Generic;
using BL;
using Persistence;
using PL_console;


namespace PL_Console
{
    public class ConsoleSecurity
    {
        Menus menu = new Menus();
        string b = "══════════════════════════════════════════════════════════════";
        public void CheckIn(User user)
        {
            char yesNo;
            do
            {
                List<Card> listcard = null;
                CardBL cardBL = new CardBL();
                Card_LogsBL cardLogsBL = new Card_LogsBL();
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
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Card card = new Card();
                    ConsoleManager manager = new ConsoleManager();
                    Card_Logs cardLogs = new Card_Logs();
                    string card_id;
                    string licensePlate;
                    string status = "";
                    var table = new ConsoleTable("Mã thẻ", "Biển số xe", "Loại thẻ", "Trạng thái");
                    foreach (var item in listcard)
                    {
                        if (item.Card_Status == 0)
                        {
                            status = "Inactive";
                        }
                        if (item.Card_Status == 1)
                        {
                            status = "Active";
                        }
                        table.AddRow(item.Card_id, item.LicensePlate, item.Card_type, status);
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
                            Console.Write("↻ Mã thẻ không tồn tại. Nhập lại: ");
                        }
                        if (card.Card_Status == 1)
                        {
                            Console.Write("↻ Thẻ đang được sử dụng. Nhập lại: ");
                        }
                    } while (card == null || card.Card_Status == 1);
                    Console.WriteLine("- Loại thẻ: " + card.Card_type);
                    if (card != null)
                    {
                        if (card.Card_type == "Thẻ ngày")
                        {
                            Console.Write("- Nhập biển số xe (VD:88-X8-8888): ");
                            do
                            {
                                licensePlate = manager.validate(5);
                                try
                                {
                                    cardLogsBL = new Card_LogsBL();
                                    cardLogs = cardLogsBL.GetCardLogsByLisencePlate(licensePlate);
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
                                    Console.Write("↻ Biển số xe đã tồn tại. Nhập lại: ");
                                }
                            } while (cardLogs != null);
                            Console.WriteLine(b);
                            Console.WriteLine();
                            Console.Write("Bạn có muốn cho xe vào không(Y/N)");
                            yesNo = menu.yesNo();
                            Console.WriteLine();
                            if (yesNo == 'Y')
                            {
                                try
                                {
                                    cardBL = new CardBL();
                                    cardBL.UpdateCardByID(new Card(null, licensePlate, null, 1, null, null), card_id);
                                    cardLogsBL = new Card_LogsBL();
                                    cardLogsBL.CreateCardLogs(new Card_Logs(card_id, user.User_name, licensePlate, DateTime.Now, null, null, null));
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
                                Console.WriteLine(b);
                                Console.WriteLine();
                                Console.WriteLine("✔ Xe đã vào bãi.");
                            }
                            if (yesNo == 'n')
                            {
                                Console.WriteLine(b);
                                Console.WriteLine();
                                Console.WriteLine("✘ Tạo thẻ không thành công.");
                            }
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine(b);
                Console.WriteLine();
                Console.Write("Bạn có muốn xe nữa vào bãi không(Y/N)");
                yesNo = menu.yesNo();
                if (yesNo == 'N')
                {
                    menu.MenuManager();
                }
            } while (yesNo != 'N');
        }
    }
}