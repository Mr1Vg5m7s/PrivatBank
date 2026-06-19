using System;
using PrivatBank.Managers;
using PrivatBank.Models;
using PrivatBank.Services;
using PrivatBank.UI;

namespace PrivatBank.Controllers
{
    internal class MenuController
    {
        private WalletService walletService;
        private ExpenseServices expenseServices;
        private IncomeServices incomeServices;
        private CurrencyService currencyService;
        private DataManager dataManager;

        public MenuController()
        {
            walletService = new WalletService();
            expenseServices = new ExpenseServices();
            incomeServices = new IncomeServices();
            currencyService = new CurrencyService();
            dataManager = new DataManager(walletService, expenseServices, incomeServices, currencyService);
                       
            if (dataManager.HasExistingData())
            {
                dataManager.LoadAllData();
                ConsoleUI.PrintSuccess("Data loaded from previous session!");
            }
            else
            {
                InitializeDefaultCurrencies();
                ConsoleUI.PrintSuccess("Initialized with default currencies!");
            }

            System.Threading.Thread.Sleep(1000);
        }

        public void Start()
        {
            while (true)
            {
                ShowMainMenu();
            }
        }

        private void ShowMainMenu()
        {
            ConsoleUI.PrintHeader("🏦 PRIVATBANK MANAGER");

            string[] options = new[]
            {
                "💰 Wallet Management",
                "💵 Income Management",
                "💸 Expense Management",
                "📊 Reports & Analytics",
                "⚙️  Settings",
                "❌ Exit"
            };

            ConsoleUI.PrintMenu(options);
            int choice = ConsoleUI.GetMenuChoice(options.Length);

            switch (choice)
            {
                case 1:
                    WalletMenu();
                    break;
                case 2:
                    IncomeMenu();
                    break;
                case 3:
                    ExpenseMenu();
                    break;
                case 4:
                    ReportsMenu();
                    break;
                case 5:
                    SettingsMenu();
                    break;
                case 6:
                    ConsoleUI.PrintSuccess("Thank you for using PrivatBank Manager!");
                    dataManager.SaveAllData();
                    Environment.Exit(0);
                    break;
            }
        }

        #region Wallet Management

        private void WalletMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("💰 WALLET MANAGEMENT");

                string[] options = new[]
                {
                    "Create Wallet",
                    "View All Wallets",
                    "Update Wallet",
                    "Delete Wallet",
                    "Back to Main Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        CreateWallet();
                        break;
                    case 2:
                        ViewAllWallets();
                        break;
                    case 3:
                        UpdateWallet();
                        break;
                    case 4:
                        DeleteWallet();
                        break;
                    case 5:
                        return;
                }
            }
        }

        private void CreateWallet()
        {
            ConsoleUI.PrintHeader("Create New Wallet");

            string name = ConsoleUI.GetStringInput("Enter wallet name: ");
            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleUI.PrintError("Wallet name cannot be empty.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("\nAvailable Currencies:");
            var currencies = currencyService.GetAllCurrencies();
            if (currencies.Count == 0)
            {
                ConsoleUI.PrintWarning("No currencies available. Please add currencies first.");
                ConsoleUI.Pause();
                return;
            }

            for (int i = 0; i < currencies.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {currencies[i].Code} - {currencies[i].Name} ({currencies[i].Symbol})");
            }

            int currencyChoice = ConsoleUI.GetIntInput("\nSelect currency (ID): ");
            var selectedCurrency = currencyService.GetCurrency(currencyChoice);

            if (selectedCurrency == null)
            {
                ConsoleUI.PrintError("Invalid currency selected.");
                ConsoleUI.Pause();
                return;
            }

            decimal balance = ConsoleUI.GetDecimalInput("Enter initial balance: ");

            var wallet = new Wallet(name, selectedCurrency, balance);
            walletService.AddWallet(wallet);
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess($"Wallet '{name}' created successfully!");
            ConsoleUI.Pause();
        }

        private void ViewAllWallets()
        {
            ConsoleUI.PrintHeader("All Wallets");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintInfo("No wallets found.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var wallet in wallets)
            {
                ConsoleUI.PrintWallet(wallet);
            }

            ConsoleUI.Pause();
        }

        private void UpdateWallet()
        {
            ConsoleUI.PrintHeader("Update Wallet");

            int walletId = ConsoleUI.GetIntInput("Enter wallet ID: ");
            var wallet = walletService.GetWallet(walletId);

            if (wallet == null)
            {
                ConsoleUI.PrintError("Wallet not found.");
                ConsoleUI.Pause();
                return;
            }

            string newName = ConsoleUI.GetStringInput($"Enter new name (current: {wallet.Name}): ");
            if (!string.IsNullOrWhiteSpace(newName))
            {
                wallet.Name = newName;
            }

            decimal newBalance = ConsoleUI.GetDecimalInput($"Enter new balance (current: {wallet.Balance:F2}): ");
            wallet.Balance = newBalance;
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess("Wallet updated successfully!");
            ConsoleUI.Pause();
        }

        private void DeleteWallet()
        {
            ConsoleUI.PrintHeader("Delete Wallet");

            ViewAllWallets();

            int walletId = ConsoleUI.GetIntInput("\nEnter wallet ID to delete: ");
            var wallet = walletService.GetWallet(walletId);

            if (wallet == null)
            {
                ConsoleUI.PrintError("Wallet not found.");
                ConsoleUI.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nAre you sure you want to delete wallet '{wallet.Name}'? (y/n): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "y")
            {
                walletService.DeleteWallet(walletId);
                dataManager.SaveAllData();
                ConsoleUI.PrintSuccess("Wallet deleted successfully!");
            }
            else
            {
                ConsoleUI.PrintInfo("Deletion cancelled.");
            }

            ConsoleUI.Pause();
        }

        #endregion

        #region Income Management

        private void IncomeMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("💵 INCOME MANAGEMENT");

                string[] options = new[]
                {
                    "Add Income Transaction",
                    "View All Income",
                    "View Income by Wallet",
                    "Delete Income Transaction",
                    "Manage Income Categories",
                    "Back to Main Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        AddIncomeTransaction();
                        break;
                    case 2:
                        ViewAllIncome();
                        break;
                    case 3:
                        ViewIncomeByWallet();
                        break;
                    case 4:
                        DeleteIncomeTransaction();
                        break;
                    case 5:
                        IncomeCategoryMenu();
                        break;
                    case 6:
                        return;
                }
            }
        }

        private void AddIncomeTransaction()
        {
            ConsoleUI.PrintHeader("Add Income Transaction");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintError("No wallets available. Create a wallet first.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("Available Wallets:");
            for (int i = 0; i < wallets.Count; i++)
            {
                Console.WriteLine($"  {wallets[i].Id}. {wallets[i].Name} ({wallets[i].Currency?.Code})");
            }

            int walletId = ConsoleUI.GetIntInput("\nSelect wallet ID: ");
            var wallet = walletService.GetWallet(walletId);

            if (wallet == null)
            {
                ConsoleUI.PrintError("Wallet not found.");
                ConsoleUI.Pause();
                return;
            }

            var categories = incomeServices.GetAllCategoriesIncome();
            if (categories.Count == 0)
            {
                ConsoleUI.PrintError("No income categories available. Create a category first.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("\nAvailable Categories:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"  {categories[i].Id}. {categories[i].Name}");
            }

            int categoryId = ConsoleUI.GetIntInput("\nSelect category ID: ");
            var category = incomeServices.GetCategoryIncome(categoryId);

            if (category == null)
            {
                ConsoleUI.PrintError("Category not found.");
                ConsoleUI.Pause();
                return;
            }

            decimal amount = ConsoleUI.GetDecimalInput("Enter income amount: ");
            var income = new Income(category, wallet, amount, DateTime.Now);
            incomeServices.AddIncome(income);
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess($"Income transaction added! New balance: {wallet.Balance:F2} {wallet.Currency?.Symbol}");
            ConsoleUI.Pause();
        }

        private void ViewAllIncome()
        {
            ConsoleUI.PrintHeader("All Income Transactions");

            var incomes = incomeServices.GetAllIncomes();
            if (incomes.Count == 0)
            {
                ConsoleUI.PrintInfo("No income transactions found.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var income in incomes)
            {
                ConsoleUI.PrintTransaction(income);
            }

            ConsoleUI.Pause();
        }

        private void ViewIncomeByWallet()
        {
            ConsoleUI.PrintHeader("View Income by Wallet");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintInfo("No wallets found.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("Available Wallets:");
            for (int i = 0; i < wallets.Count; i++)
            {
                Console.WriteLine($"  {wallets[i].Id}. {wallets[i].Name}");
            }

            int walletId = ConsoleUI.GetIntInput("\nSelect wallet ID: ");
            var incomes = incomeServices.GetIncomesByWallet(walletId);

            if (incomes.Count == 0)
            {
                ConsoleUI.PrintInfo("No income transactions found for this wallet.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var income in incomes)
            {
                ConsoleUI.PrintTransaction(income);
            }

            ConsoleUI.Pause();
        }

        private void DeleteIncomeTransaction()
        {
            ConsoleUI.PrintHeader("Delete Income Transaction");

            ViewAllIncome();

            int incomeId = ConsoleUI.GetIntInput("\nEnter income transaction ID to delete: ");
            var income = incomeServices.GetIncome(incomeId);

            if (income == null)
            {
                ConsoleUI.PrintError("Income transaction not found.");
                ConsoleUI.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nAre you sure you want to delete this transaction? (y/n): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "y")
            {
                incomeServices.DeleteIncome(incomeId);
                dataManager.SaveAllData();
                ConsoleUI.PrintSuccess("Income transaction deleted!");
            }
            else
            {
                ConsoleUI.PrintInfo("Deletion cancelled.");
            }

            ConsoleUI.Pause();
        }

        private void IncomeCategoryMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("📂 Income Categories");

                string[] options = new[]
                {
                    "Add Category",
                    "View Categories",
                    "Delete Category",
                    "Back to Income Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        AddIncomeCategory();
                        break;
                    case 2:
                        ViewIncomeCategories();
                        break;
                    case 3:
                        DeleteIncomeCategory();
                        break;
                    case 4:
                        return;
                }
            }
        }

        private void AddIncomeCategory()
        {
            ConsoleUI.PrintHeader("Add Income Category");

            string categoryName = ConsoleUI.GetStringInput("Enter category name: ");
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                ConsoleUI.PrintError("Category name cannot be empty.");
                ConsoleUI.Pause();
                return;
            }

            var category = new CategoryIncome(0, categoryName);
            incomeServices.AddCategoryIncome(category);
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess($"Income category '{categoryName}' added successfully!");
            ConsoleUI.Pause();
        }

        private void ViewIncomeCategories()
        {
            ConsoleUI.PrintHeader("Income Categories");

            var categories = incomeServices.GetAllCategoriesIncome();
            if (categories.Count == 0)
            {
                ConsoleUI.PrintInfo("No income categories found.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var category in categories)
            {
                Console.WriteLine($"  ID: {category.Id} - {category.Name}");
            }

            ConsoleUI.Pause();
        }

        private void DeleteIncomeCategory()
        {
            ConsoleUI.PrintHeader("Delete Income Category");

            ViewIncomeCategories();

            int categoryId = ConsoleUI.GetIntInput("\nEnter category ID to delete: ");
            var category = incomeServices.GetCategoryIncome(categoryId);

            if (category == null)
            {
                ConsoleUI.PrintError("Category not found.");
                ConsoleUI.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nAre you sure you want to delete category '{category.Name}'? (y/n): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "y")
            {
                incomeServices.DeleteCategory(categoryId);
                dataManager.SaveAllData();
                ConsoleUI.PrintSuccess("Category deleted successfully!");
            }
            else
            {
                ConsoleUI.PrintInfo("Deletion cancelled.");
            }

            ConsoleUI.Pause();
        }

        #endregion

        #region Expense Management

        private void ExpenseMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("💸 EXPENSE MANAGEMENT");

                string[] options = new[]
                {
                    "Add Expense Transaction",
                    "View All Expenses",
                    "View Expenses by Wallet",
                    "Delete Expense Transaction",
                    "Manage Expense Categories",
                    "Back to Main Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        AddExpenseTransaction();
                        break;
                    case 2:
                        ViewAllExpenses();
                        break;
                    case 3:
                        ViewExpensesByWallet();
                        break;
                    case 4:
                        DeleteExpenseTransaction();
                        break;
                    case 5:
                        ExpenseCategoryMenu();
                        break;
                    case 6:
                        return;
                }
            }
        }

        private void AddExpenseTransaction()
        {
            ConsoleUI.PrintHeader("Add Expense Transaction");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintError("No wallets available. Create a wallet first.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("Available Wallets:");
            for (int i = 0; i < wallets.Count; i++)
            {
                Console.WriteLine($"  {wallets[i].Id}. {wallets[i].Name} ({wallets[i].Currency?.Code})");
            }

            int walletId = ConsoleUI.GetIntInput("\nSelect wallet ID: ");
            var wallet = walletService.GetWallet(walletId);

            if (wallet == null)
            {
                ConsoleUI.PrintError("Wallet not found.");
                ConsoleUI.Pause();
                return;
            }

            var categories = expenseServices.GetAllCategoriesExpense();
            if (categories.Count == 0)
            {
                ConsoleUI.PrintError("No expense categories available. Create a category first.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("\nAvailable Categories:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"  {categories[i].Id}. {categories[i].Name}");
            }

            int categoryId = ConsoleUI.GetIntInput("\nSelect category ID: ");
            var category = expenseServices.GetCategoryExpense(categoryId);

            if (category == null)
            {
                ConsoleUI.PrintError("Category not found.");
                ConsoleUI.Pause();
                return;
            }

            decimal amount = ConsoleUI.GetDecimalInput("Enter expense amount: ");

            if (wallet.Balance < amount)
            {
                ConsoleUI.PrintError($"Insufficient balance! Available: {wallet.Balance:F2} {wallet.Currency?.Symbol}");
                ConsoleUI.Pause();
                return;
            }

            var expense = new Expense(category, wallet, amount, DateTime.Now);
            expenseServices.AddExpense(expense);
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess($"Expense transaction added! New balance: {wallet.Balance:F2} {wallet.Currency?.Symbol}");
            ConsoleUI.Pause();
        }

        private void ViewAllExpenses()
        {
            ConsoleUI.PrintHeader("All Expense Transactions");

            var expenses = expenseServices.GetAllExpenses();
            if (expenses.Count == 0)
            {
                ConsoleUI.PrintInfo("No expense transactions found.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var expense in expenses)
            {
                ConsoleUI.PrintTransaction(expense);
            }

            ConsoleUI.Pause();
        }

        private void ViewExpensesByWallet()
        {
            ConsoleUI.PrintHeader("View Expenses by Wallet");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintInfo("No wallets found.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("Available Wallets:");
            for (int i = 0; i < wallets.Count; i++)
            {
                Console.WriteLine($"  {wallets[i].Id}. {wallets[i].Name}");
            }

            int walletId = ConsoleUI.GetIntInput("\nSelect wallet ID: ");
            var expenses = expenseServices.GetExpensesByWallet(walletId);

            if (expenses.Count == 0)
            {
                ConsoleUI.PrintInfo("No expense transactions found for this wallet.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var expense in expenses)
            {
                ConsoleUI.PrintTransaction(expense);
            }

            ConsoleUI.Pause();
        }

        private void DeleteExpenseTransaction()
        {
            ConsoleUI.PrintHeader("Delete Expense Transaction");

            ViewAllExpenses();

            int expenseId = ConsoleUI.GetIntInput("\nEnter expense transaction ID to delete: ");
            var expense = expenseServices.GetExpense(expenseId);

            if (expense == null)
            {
                ConsoleUI.PrintError("Expense transaction not found.");
                ConsoleUI.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nAre you sure you want to delete this transaction? (y/n): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "y")
            {
                expenseServices.DeletedExpense(expenseId);
                dataManager.SaveAllData();
                ConsoleUI.PrintSuccess("Expense transaction deleted!");
            }
            else
            {
                ConsoleUI.PrintInfo("Deletion cancelled.");
            }

            ConsoleUI.Pause();
        }

        private void ExpenseCategoryMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("📂 Expense Categories");

                string[] options = new[]
                {
                    "Add Category",
                    "View Categories",
                    "Delete Category",
                    "Back to Expense Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        AddExpenseCategory();
                        break;
                    case 2:
                        ViewExpenseCategories();
                        break;
                    case 3:
                        DeleteExpenseCategory();
                        break;
                    case 4:
                        return;
                }
            }
        }

        private void AddExpenseCategory()
        {
            ConsoleUI.PrintHeader("Add Expense Category");

            string categoryName = ConsoleUI.GetStringInput("Enter category name: ");
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                ConsoleUI.PrintError("Category name cannot be empty.");
                ConsoleUI.Pause();
                return;
            }

            var category = new CategoryExpense(0, categoryName);
            expenseServices.AddCategoryExpense(category);
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess($"Expense category '{categoryName}' added successfully!");
            ConsoleUI.Pause();
        }

        private void ViewExpenseCategories()
        {
            ConsoleUI.PrintHeader("Expense Categories");

            var categories = expenseServices.GetAllCategoriesExpense();
            if (categories.Count == 0)
            {
                ConsoleUI.PrintInfo("No expense categories found.");
                ConsoleUI.Pause();
                return;
            }

            foreach (var category in categories)
            {
                Console.WriteLine($"  ID: {category.Id} - {category.Name}");
            }

            ConsoleUI.Pause();
        }

        private void DeleteExpenseCategory()
        {
            ConsoleUI.PrintHeader("Delete Expense Category");

            ViewExpenseCategories();

            int categoryId = ConsoleUI.GetIntInput("\nEnter category ID to delete: ");
            var category = expenseServices.GetCategoryExpense(categoryId);

            if (category == null)
            {
                ConsoleUI.PrintError("Category not found.");
                ConsoleUI.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nAre you sure you want to delete category '{category.Name}'? (y/n): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "y")
            {
                expenseServices.DeleteCategoryExpense(categoryId);
                dataManager.SaveAllData();
                ConsoleUI.PrintSuccess("Category deleted successfully!");
            }
            else
            {
                ConsoleUI.PrintInfo("Deletion cancelled.");
            }

            ConsoleUI.Pause();
        }

        #endregion

        #region Reports & Analytics

        private void ReportsMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("📊 REPORTS & ANALYTICS");

                string[] options = new[]
                {
                    "Overall Balance Summary",
                    "Income vs Expense Report",
                    "Expenses by Category",
                    "Income by Category",
                    "Wallet Comparison",
                    "Back to Main Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        OverallBalanceSummary();
                        break;
                    case 2:
                        IncomeVsExpenseReport();
                        break;
                    case 3:
                        ExpensesByCategory();
                        break;
                    case 4:
                        IncomeByCategory();
                        break;
                    case 5:
                        WalletComparison();
                        break;
                    case 6:
                        return;
                }
            }
        }

        private void OverallBalanceSummary()
        {
            ConsoleUI.PrintHeader("Overall Balance Summary");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintInfo("No wallets found.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║         WALLET SUMMARY REPORT          ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            decimal totalBalance = 0;
            foreach (var wallet in wallets)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  {wallet.Name}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Balance: {wallet.Balance:F2} {wallet.Currency?.Symbol}");
                Console.ResetColor();
                totalBalance += wallet.Balance;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  Total Balance: {totalBalance:F2}");
            Console.ResetColor();

            ConsoleUI.Pause();
        }

        private void IncomeVsExpenseReport()
        {
            ConsoleUI.PrintHeader("Income vs Expense Report");

            var incomes = incomeServices.GetAllIncomes();
            var expenses = expenseServices.GetAllExpenses();

            decimal totalIncome = incomes.Sum(i => i.Sum);
            decimal totalExpense = expenses.Sum(e => e.Sum);
            decimal netAmount = totalIncome - totalExpense;

            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║      INCOME VS EXPENSE REPORT          ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  Total Income:  +{totalIncome:F2}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Total Expense: -{totalExpense:F2}");
            Console.ResetColor();

            Console.WriteLine();
            if (netAmount >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Net Amount:    +{netAmount:F2}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  Net Amount:    {netAmount:F2}");
            }
            Console.ResetColor();

            ConsoleUI.Pause();
        }

        private void ExpensesByCategory()
        {
            ConsoleUI.PrintHeader("Expenses by Category");

            var expenses = expenseServices.GetAllExpenses();
            if (expenses.Count == 0)
            {
                ConsoleUI.PrintInfo("No expenses found.");
                ConsoleUI.Pause();
                return;
            }

            var groupedByCategory = expenses.GroupBy(e => e.Category?.Name ?? "Uncategorized")
                .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Sum) })
                .OrderByDescending(x => x.Total);

            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║      EXPENSES BY CATEGORY REPORT       ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            foreach (var group in groupedByCategory)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {group.Category}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  Total: -{group.Total:F2}");
                Console.ResetColor();
                Console.WriteLine();
            }

            ConsoleUI.Pause();
        }

        private void IncomeByCategory()
        {
            ConsoleUI.PrintHeader("Income by Category");

            var incomes = incomeServices.GetAllIncomes();
            if (incomes.Count == 0)
            {
                ConsoleUI.PrintInfo("No income found.");
                ConsoleUI.Pause();
                return;
            }

            var groupedByCategory = incomes.GroupBy(i => i.Category?.Name ?? "Uncategorized")
                .Select(g => new { Category = g.Key, Total = g.Sum(i => i.Sum) })
                .OrderByDescending(x => x.Total);

            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║       INCOME BY CATEGORY REPORT        ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            foreach (var group in groupedByCategory)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {group.Category}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Total: +{group.Total:F2}");
                Console.ResetColor();
                Console.WriteLine();
            }

            ConsoleUI.Pause();
        }

        private void WalletComparison()
        {
            ConsoleUI.PrintHeader("Wallet Comparison");

            var wallets = walletService.GetAllWallets();
            if (wallets.Count == 0)
            {
                ConsoleUI.PrintInfo("No wallets found.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine("║       WALLET COMPARISON REPORT         ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");

            var sortedWallets = wallets.OrderByDescending(w => w.Balance);

            foreach (var wallet in sortedWallets)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  {wallet.Name}");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"  ID: {wallet.Id} | Currency: {wallet.Currency?.Code}");

                if (wallet.Balance > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (wallet.Balance < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine($"  Balance: {wallet.Balance:F2} {wallet.Currency?.Symbol}");
                Console.ResetColor();
                Console.WriteLine();
            }

            ConsoleUI.Pause();
        }

        #endregion

        #region Settings

        private void SettingsMenu()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("⚙️  SETTINGS");

                string[] options = new[]
                {
                    "Manage Currencies",
                    "View All Transactions",
                    "Clear All Data",
                    "Back to Main Menu"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        ManageCurrencies();
                        break;
                    case 2:
                        ViewAllTransactions();
                        break;
                    case 3:
                        ClearAllDataMenu();
                        break;
                    case 4:
                        return;
                }
            }
        }

        private void ManageCurrencies()
        {
            while (true)
            {
                ConsoleUI.PrintHeader("Manage Currencies");

                string[] options = new[]
                {
                    "Add Currency",
                    "View All Currencies",
                    "Delete Currency",
                    "Back to Settings"
                };

                ConsoleUI.PrintMenu(options);
                int choice = ConsoleUI.GetMenuChoice(options.Length);

                switch (choice)
                {
                    case 1:
                        AddCurrency();
                        break;
                    case 2:
                        ViewCurrencies();
                        break;
                    case 3:
                        DeleteCurrency();
                        break;
                    case 4:
                        return;
                }
            }
        }

        private void AddCurrency()
        {
            ConsoleUI.PrintHeader("Add Currency");

            string code = ConsoleUI.GetStringInput("Enter currency code (e.g., USD, EUR): ").ToUpper();
            if (string.IsNullOrWhiteSpace(code))
            {
                ConsoleUI.PrintError("Currency code cannot be empty.");
                ConsoleUI.Pause();
                return;
            }

            string name = ConsoleUI.GetStringInput("Enter currency name (e.g., US Dollar): ");
            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleUI.PrintError("Currency name cannot be empty.");
                ConsoleUI.Pause();
                return;
            }

            string symbol = ConsoleUI.GetStringInput("Enter currency symbol (e.g., $): ");
            if (string.IsNullOrWhiteSpace(symbol))
            {
                ConsoleUI.PrintError("Currency symbol cannot be empty.");
                ConsoleUI.Pause();
                return;
            }

            var currency = new Currency(code, name, symbol);
            currencyService.AddCurrency(currency);
            dataManager.SaveAllData();

            ConsoleUI.PrintSuccess($"Currency '{code}' added successfully!");
            ConsoleUI.Pause();
        }

        private void ViewCurrencies()
        {
            ConsoleUI.PrintHeader("All Currencies");

            var currencies = currencyService.GetAllCurrencies();
            if (currencies.Count == 0)
            {
                ConsoleUI.PrintInfo("No currencies found.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine();
            foreach (var currency in currencies)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  ID: {currency.Id}");
                Console.WriteLine($"  Code: {currency.Code}");
                Console.WriteLine($"  Name: {currency.Name}");
                Console.WriteLine($"  Symbol: {currency.Symbol}");
                Console.ResetColor();
                Console.WriteLine();
            }

            ConsoleUI.Pause();
        }

        private void DeleteCurrency()
        {
            ConsoleUI.PrintHeader("Delete Currency");

            ViewCurrencies();

            int currencyId = ConsoleUI.GetIntInput("\nEnter currency ID to delete: ");
            var currency = currencyService.GetCurrency(currencyId);

            if (currency == null)
            {
                ConsoleUI.PrintError("Currency not found.");
                ConsoleUI.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nAre you sure you want to delete currency '{currency.Code}'? (y/n): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "y")
            {
                currencyService.DeleteCurrency(currencyId);
                dataManager.SaveAllData();
                ConsoleUI.PrintSuccess("Currency deleted successfully!");
            }
            else
            {
                ConsoleUI.PrintInfo("Deletion cancelled.");
            }

            ConsoleUI.Pause();
        }

        private void ViewAllTransactions()
        {
            ConsoleUI.PrintHeader("All Transactions");

            var incomes = incomeServices.GetAllIncomes();
            var expenses = expenseServices.GetAllExpenses();

            if (incomes.Count == 0 && expenses.Count == 0)
            {
                ConsoleUI.PrintInfo("No transactions found.");
                ConsoleUI.Pause();
                return;
            }

            Console.WriteLine("\n══════ INCOME TRANSACTIONS ══════\n");
            if (incomes.Count == 0)
            {
                ConsoleUI.PrintInfo("No income transactions.");
            }
            else
            {
                foreach (var income in incomes)
                {
                    ConsoleUI.PrintTransaction(income);
                }
            }

            Console.WriteLine("\n══════ EXPENSE TRANSACTIONS ══════\n");
            if (expenses.Count == 0)
            {
                ConsoleUI.PrintInfo("No expense transactions.");
            }
            else
            {
                foreach (var expense in expenses)
                {
                    ConsoleUI.PrintTransaction(expense);
                }
            }

            ConsoleUI.Pause();
        }

        private void ClearAllDataMenu()
        {
            ConsoleUI.PrintHeader("⚠️  CLEAR ALL DATA");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️  WARNING: This will permanently delete ALL data!");
            Console.WriteLine("This action CANNOT be undone.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nAre you absolutely sure? (type 'yes' to confirm): ");
            Console.ResetColor();

            string confirm = Console.ReadLine()?.ToLower() ?? "";
            if (confirm == "yes")
            {
                dataManager.ClearAllData();
                ConsoleUI.PrintSuccess("All data has been cleared!");
                Console.WriteLine("\nApplication will restart with default settings...");
                System.Threading.Thread.Sleep(2000);
                Environment.Exit(0);
            }
            else
            {
                ConsoleUI.PrintInfo("Clear data cancelled.");
            }

            ConsoleUI.Pause();
        }

        #endregion

        #region Initialization

        private void InitializeDefaultCurrencies()
        {
            currencyService.AddCurrency(new Currency("USD", "US Dollar", "$"));
            currencyService.AddCurrency(new Currency("EUR", "Euro", "€"));
            currencyService.AddCurrency(new Currency("UAH", "Ukrainian Hryvnia", "₴"));
            currencyService.AddCurrency(new Currency("GBP", "British Pound", "£"));
            dataManager.SaveAllData();
        }

        #endregion
    }
}
