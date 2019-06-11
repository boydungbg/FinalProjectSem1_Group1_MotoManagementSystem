using System;

namespace PL_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Menus menu = new Menus();
            Console.ForegroundColor = ConsoleColor.White;
            menu.MenuChoice();
            Console.ResetColor();
        }
    }
}
