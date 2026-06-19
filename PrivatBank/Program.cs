using System;
using PrivatBank.Controllers;

namespace PrivatBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var menuController = new MenuController();
            menuController.Start();
        }
    }
}