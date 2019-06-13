using System;
using System.Text.RegularExpressions;
using BL;
using Persistence;
using PL_console;

namespace PL_Console
{
    public class Menus
    {
        string b = "══════════════════════════════════════════════════════════════";
        public void MenuChoice()
        {
            char choose;
            do
            {
                Console.Clear();
                string[] menuItem = { "Đăng nhập", "Thoát chương trình" };
                choose = Menu(menuItem, 2, "HỆ THỐNG QUẢN LÝ BÃI GỬI XE MÁY", "#Chọn: ");
                switch (choose)
                {
                    case '1':
                        MenuLogin();
                        break;
                    case '2':
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            } while (choose != '2');

        }
        public char Menu(string[] menuItem, int itemCount, string tittle, string choice)
        {
            char a;
            while (true)
            {
                Console.WriteLine(b);
                Console.WriteLine(" " + tittle);
                Console.WriteLine(b);
                for (int i = 0; i < itemCount; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, menuItem[i]);
                }
                Console.WriteLine(b);
                Console.Write(choice);
                try
                {
                    a = Convert.ToChar(Console.ReadLine());
                }
                catch (System.Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("Bạn nhập sai!!!Vui lòng nhập lai....!");
                    continue;
                }
                Console.WriteLine();
                break;
            }
            return a;
        }
        public void MenuLogin()
        {
            User user = null;
            string accname;
            string accpass;
            while (true)
            {
                Console.Clear();
                char choice;
                Console.WriteLine(b);
                Console.WriteLine(" ĐĂNG NHẬP");
                Console.WriteLine(b);
                Console.Write(" Nhập tài khoản : ");
                accname = Console.ReadLine();
                Console.Write(" Nhập mật khẩu  : ");
                accpass = password();
                try
                {
                    UserBL userbl = new UserBL();
                    user = userbl.Login(accname, accpass);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(b);
                    Console.WriteLine(ex.Message);
                    Console.Write("Lỗi!!! Bạn có muốn đăng nhập lại không? (Y/N)");
                    choice = yesNo();
                    if (choice == 'Y')
                    {
                        MenuLogin();
                    }
                    if (choice == 'N')
                    {
                        MenuChoice();
                    }
                }
                if ((validate(accname) == false) || (validate(accpass) == false))
                {
                    Console.WriteLine();
                    Console.WriteLine(b);
                    Console.Write("Tên đăng nhập / mật khẩu không được chứa kí tự đặc biệt, bạn có muốn đăng nhập lại không? (Y/N)");
                    choice = yesNo();
                    if (choice == 'Y')
                    {
                        MenuLogin();
                    }
                    if (choice == 'N')
                    {
                        MenuChoice();
                    }
                }
                if (user == null)
                {
                    Console.WriteLine();
                    Console.WriteLine(b);
                    Console.Write("Tên đăng nhập / mật khẩu không đúng, bạn có muốn đăng nhập lại không? (Y/N)");
                    choice = yesNo();
                    if (choice == 'Y')
                    {
                        MenuLogin();
                    }
                    if (choice == 'N')
                    {
                        MenuChoice();
                    }
                }
                break;
            }
            if (user.User_level == 0)
            {
                MenuManager(user);
            }
            if (user.User_level == 1)
            {
                MenuSecurity(user);
            }
        }
        public string password()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return pass;
        }
        public bool validate(string str)
        {
            Regex regex = new Regex("[a-zA-Z0-9_]");
            MatchCollection matchCollectionstr = regex.Matches(str);
            if (matchCollectionstr.Count < str.Length)
            {
                return false;
            }
            return true;
        }
        public void MenuManager(User user)
        {
            char choose;
            do
            {
                Console.Clear();
                ManagerCardAndStatistic manager = new ManagerCardAndStatistic();
                string[] menuItem = { "Tạo thẻ tháng", "Hiển thị danh sách thẻ tháng", "Thống kê", "Đăng xuất" };
                choose = Menu(menuItem, 4, "Chào mừng bạn đã đến với hệ thống quản lý bãi gửi xe", "#Chọn: ");
                switch (choose)
                {
                    case '1':
                        manager.CreateCard(user);
                        break;
                    case '2':
                        manager.DisplayListCardsMonth(0, user);
                        break;
                    case '3':
                        MenuStatictis(user);
                        break;
                    case '4':
                        MenuChoice();
                        break;
                    default:
                        Console.WriteLine("Bạn nhập sai!!!Vui lòng nhập lai....!");
                        break;
                }
            } while (choose != '4');
        }
        public void MenuStatictis(User user)
        {
            char choose;
            do
            {
                Console.Clear();
                ManagerCardAndStatistic manager = new ManagerCardAndStatistic();
                string[] menuItem = { "Thống kê xe ra vào thẻ ngày", "Thống kê xe ra vào thẻ tháng", "Thống kê xe ra vào bằng biển số xe", "Quay lại" };
                choose = Menu(menuItem, 4, "Chào mừng bạn đã đến với hệ thống quản lý bãi gửi xe", "#Chọn: ");
                switch (choose)
                {
                    case '1':
                        manager.Statistical(user, 1);
                        break;
                    case '2':
                        manager.Statistical(user, 2);
                        break;
                    case '3':
                        manager.Statistical(user, 3);
                        break;
                    case '4':
                        MenuManager(user);
                        break;
                    default:
                        Console.WriteLine("Bạn nhập sai!!!Vui lòng nhập lai....!");
                        break;
                }
            } while (choose != '4');
        }
        public void MenuSecurity(User user)
        {
            SecurityCheckInCheckOut security = new SecurityCheckInCheckOut();
            char choose;
            do
            {
                Console.Clear();
                string[] menuItem = { "Kiểm tra xe vào", "Kiểm tra xe ra", "Đăng xuất" };
                choose = Menu(menuItem, 3, "Kiểm tra xe vào xe ra", "#Chọn: ");
                switch (choose)
                {
                    case '1':
                        security.Check(user, 1);
                        break;
                    case '2':
                        security.Check(user, 2);
                        break;
                    case '3':
                        MenuChoice();
                        break;
                    default:
                        break;
                }
            } while (choose != '3');
        }
        public char yesNo()
        {
            char ys;
            do
            {
                try
                {
                    ys = Convert.ToChar(Console.ReadLine().ToUpper());
                }
                catch (System.Exception)
                {
                    Console.WriteLine();
                    Console.Write("Bạn chỉ được nhập (Y/N): ");
                    continue;
                }
                if (ys != 'Y' && ys != 'N')
                {
                    Console.WriteLine();
                    Console.Write("Bạn chỉ được nhập (Y/N): ");
                    continue;
                }
                break;
            } while (true || ys != 'N' && ys != 'Y');
            return ys;
        }
    }
}