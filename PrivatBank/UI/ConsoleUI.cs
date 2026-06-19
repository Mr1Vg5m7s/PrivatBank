using System;
using PrivatBank.Models;

namespace PrivatBank.UI
{
    internal class ConsoleUI
    {
        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═════════════════════════════════════════════════════");
            Console.WriteLine($"  {title}");
            Console.WriteLine("═════════════════════════════════════════════════════");
            Console.ResetColor();
        }

        public static void PrintMenu(string[] options)
        {
            Console.WriteLine();
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {options[i]}");
            }
            Console.WriteLine();
        }

        public static int GetMenuChoice(int maxOption)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Enter your choice: ");
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= maxOption)
                {
                    return choice;
                }

                PrintError("Invalid choice. Please try again.");
            }
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ {message}");
            Console.ResetColor();
        }

        public static void PrintInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"ℹ {message}");
            Console.ResetColor();
        }

        public static string GetStringInput(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(prompt);
            Console.ResetColor();
            return Console.ReadLine() ?? string.Empty;
        }

        public static decimal GetDecimalInput(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();

                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= 0)
                {
                    return value;
                }

                PrintError("Invalid input. Please enter a positive number.");
            }
        }

        public static int GetIntInput(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(prompt);
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                {
                    return value;
                }

                PrintError("Invalid input. Please enter a positive number.");
            }
        }

        public static void PrintWallet(Wallet wallet)
        {
            if (wallet == null) return;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n  ID: {wallet.Id}");
            Console.WriteLine($"  Name: {wallet.Name}");
            Console.WriteLine($"  Currency: {wallet.Currency?.Code} ({wallet.Currency?.Symbol})");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  Balance: {wallet.Balance:F2} {wallet.Currency?.Symbol}");
            Console.ResetColor();
        }

        public static void PrintTransaction(Expense expense)
        {
            if (expense == null) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  ID: {expense.Id}");
            Console.WriteLine($"  Category: {expense.Category?.Name}");
            Console.WriteLine($"  Wallet: {expense.Wallet?.Name}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Amount: -{expense.Sum:F2} {expense.Wallet?.Currency?.Symbol}");
            Console.WriteLine($"  Date: {expense.Date:yyyy-MM-dd HH:mm:ss}");
            Console.ResetColor();
        }

        public static void PrintTransaction(Income income)
        {
            if (income == null) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  ID: {income.Id}");
            Console.WriteLine($"  Category: {income.Category?.Name}");
            Console.WriteLine($"  Wallet: {income.Wallet?.Name}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  Amount: +{income.Sum:F2} {income.Wallet?.Currency?.Symbol}");
            Console.WriteLine($"  Date: {income.Date:yyyy-MM-dd HH:mm:ss}");
            Console.ResetColor();
        }

        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
