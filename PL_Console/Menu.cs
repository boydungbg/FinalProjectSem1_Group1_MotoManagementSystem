using System;
using System.Text.RegularExpressions;
using BL;
using Persistence;
using PL_console;

namespace PL_Console
{
    public class Menus
    {
        private User user = null;
        string b = "══════════════════════════════════════════════════════════════";
        public void MenuChoice()
        {
            Console.Clear();
            string[] menuItem = { "Đăng nhập", "Thoát chương trình" };
            char choose = Menu(menuItem, 2, "HỆ THỐNG QUẢN LÝ BÃI GỬI XE MÁY", "#Chọn: ");
            switch (choose)
            {
                case '1':
                    MenuLogin();
                    break;
                case '2':
                    Environment.Exit(0);
                    break;
            }
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
            UserBL userbl = new UserBL();
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
                    user = userbl.Login(accname, accpass);
                }
                catch (System.Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine(b);
                    Console.Write("Mất kết nối, bạn có muốn đăng nhập lại không? (Y/N)");
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
                MenuManager();
            }
            if (user.User_level == 1)
            {
                MenuSecurity();
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
        public void MenuManager()
        {
            Console.Clear();
            ConsoleManager manager = new ConsoleManager();
            string[] menuItem = { "Tạo thẻ tháng", "Thống kê", "Đăng xuất" };
            char choose = Menu(menuItem, 3, "Chào mừng bạn đã đến với hệ thống quản lý bãi gửi xe", "#Chọn: ");
            switch (choose)
            {
                case '1':
                    manager.CreateCard();
                    break;
                case '2':
                    MenuStatictis();
                    break;
                case '3':
                    MenuLogin();
                    break;
            }
        }
        public void MenuStatictis()
        {
            Console.Clear();
            string[] menuItem = { "Thống kê theo ngày", "Thống kê theo tháng", "Quay lại" };
            char choose = Menu(menuItem, 3, "Thống kê", "#Chọn: ");
            switch (choose)
            {
                case '1':

                    break;
                case '2':
                    Console.WriteLine("Thống kê");
                    break;
                case '3':
                    MenuManager();
                    break;
            }
        }
        public void MenuSecurity()
        {
            Console.Clear();
            string[] menuItem = { "Kiểm tra xe ra vào", "Đăng xuất" };
            char choose = Menu(menuItem, 2, "Chào mừng bạn đã đến với hệ thống quản lý bãi gửi xe", "#Chọn: ");
            switch (choose)
            {
                case '1':
                    CheckInCheckOut();
                    break;
                case '2':
                    MenuLogin();
                    break;
            }
        }
        public void CheckInCheckOut()
        {
            ConsoleSecurity security = new ConsoleSecurity();
            Console.Clear();
            string[] menuItem = { "Kiểm tra xe vào", "Kiểm tra xe ra", "Quay lại" };
            char choose = Menu(menuItem, 3, "Kiểm tra xe vào xe ra", "#Chọn: ");
            switch (choose)
            {
                case '1':
                    security.CheckIn(user);
                    break;
                case '2':
                    Console.WriteLine("Xe ra - xe vào");
                    break;
                case '3':
                    MenuSecurity();
                    break;
            }
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