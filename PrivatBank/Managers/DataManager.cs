using System;
using System.Collections.Generic;
using System.Linq;
using PrivatBank.Data;
using PrivatBank.Models;
using PrivatBank.Services;

namespace PrivatBank.Managers
{
    internal class DataManager
    {
        private readonly WalletService walletService;
        private readonly ExpenseServices expenseServices;
        private readonly IncomeServices incomeServices;
        private readonly CurrencyService currencyService;
        private readonly DataPersistence dataPersistence;

        public DataManager(
            WalletService walletService,
            ExpenseServices expenseServices,
            IncomeServices incomeServices,
            CurrencyService currencyService)
        {
            this.walletService = walletService;
            this.expenseServices = expenseServices;
            this.incomeServices = incomeServices;
            this.currencyService = currencyService;
            this.dataPersistence = new DataPersistence();
        }

        /// <summary>
        /// Load all data from JSON files
        /// </summary>
        public void LoadAllData()
        {
            // Load currencies first (needed by wallets)
            LoadCurrencies();

            // Load wallets
            LoadWallets();

            // Load categories
            LoadExpenseCategories();
            LoadIncomeCategories();

            // Load transactions
            LoadExpenses();
            LoadIncomes();
        }

        /// <summary>
        /// Save all data to JSON files
        /// </summary>
        public void SaveAllData()
        {
            SaveWallets();
            SaveExpenses();
            SaveIncomes();
            SaveExpenseCategories();
            SaveIncomeCategories();
            SaveCurrencies();
        }

        #region Wallet Management

        private void LoadWallets()
        {
            var wallets = dataPersistence.LoadWallets();
            foreach (var wallet in wallets)
            {
                walletService.AddWallet(wallet);
            }
        }

        private void SaveWallets()
        {
            // Get reflection to access private wallets list
            var wallets = GetPrivateField<List<Wallet>>(walletService, "wallets");
            if (wallets != null)
            {
                dataPersistence.SaveWallets(wallets);
            }
        }

        #endregion

        #region Expense Management

        private void LoadExpenses()
        {
            var expenseDataList = dataPersistence.LoadExpenses();
            var expenses = GetPrivateField<List<Expense>>(expenseServices, "expenses") ?? new List<Expense>();
            var categories = GetPrivateField<List<CategoryExpense>>(expenseServices, "categories") ?? new List<CategoryExpense>();

            foreach (var expenseData in expenseDataList)
            {
                var category = categories.FirstOrDefault(c => c.Id == expenseData.CategoryId);
                var wallet = walletService.GetWallet(expenseData.WalletId);

                if (category != null && wallet != null)
                {
                    var expense = new Expense(category, wallet, expenseData.Sum, expenseData.Date);
                    // Manually set the ID to match loaded data
                    SetPrivateField(expense, "Id", expenseData.Id);
                    expenses.Add(expense);
                }
            }
        }

        private void SaveExpenses()
        {
            var expenses = GetPrivateField<List<Expense>>(expenseServices, "expenses");
            if (expenses != null)
            {
                dataPersistence.SaveExpenses(expenses);
            }
        }

        private void LoadExpenseCategories()
        {
            var categories = dataPersistence.LoadExpenseCategories();
            foreach (var category in categories)
            {
                expenseServices.AddCategoryExpense(category);
            }
        }

        private void SaveExpenseCategories()
        {
            var categories = GetPrivateField<List<CategoryExpense>>(expenseServices, "categories");
            if (categories != null)
            {
                dataPersistence.SaveExpenseCategories(categories);
            }
        }

        #endregion

        #region Income Management

        private void LoadIncomes()
        {
            var incomeDataList = dataPersistence.LoadIncomes();
            var incomes = GetPrivateField<List<Income>>(incomeServices, "incomes") ?? new List<Income>();
            var categories = GetPrivateField<List<CategoryIncome>>(incomeServices, "categories") ?? new List<CategoryIncome>();

            foreach (var incomeData in incomeDataList)
            {
                var category = categories.FirstOrDefault(c => c.Id == incomeData.CategoryId);
                var wallet = walletService.GetWallet(incomeData.WalletId);

                if (category != null && wallet != null)
                {
                    var income = new Income(category, wallet, incomeData.Sum, incomeData.Date);
                    // Manually set the ID to match loaded data
                    SetPrivateField(income, "Id", incomeData.Id);
                    incomes.Add(income);
                }
            }
        }

        private void SaveIncomes()
        {
            var incomes = GetPrivateField<List<Income>>(incomeServices, "incomes");
            if (incomes != null)
            {
                dataPersistence.SaveIncomes(incomes);
            }
        }

        private void LoadIncomeCategories()
        {
            var categories = dataPersistence.LoadIncomeCategories();
            foreach (var category in categories)
            {
                incomeServices.AddCategoryIncome(category);
            }
        }

        private void SaveIncomeCategories()
        {
            var categories = GetPrivateField<List<CategoryIncome>>(incomeServices, "categories");
            if (categories != null)
            {
                dataPersistence.SaveIncomeCategories(categories);
            }
        }

        #endregion

        #region Currency Management

        private void LoadCurrencies()
        {
            var currencies = dataPersistence.LoadCurrencies();
            foreach (var currency in currencies)
            {
                currencyService.AddCurrency(currency);
            }
        }

        private void SaveCurrencies()
        {
            var currencies = GetPrivateField<List<Currency>>(currencyService, "currencies");
            if (currencies != null)
            {
                dataPersistence.SaveCurrencies(currencies);
            }
        }

        #endregion

        #region Reflection Helpers

        /// <summary>
        /// Helper method to get private field values using reflection
        /// </summary>
        private T? GetPrivateField<T>(object obj, string fieldName)
        {
            try
            {
                var field = obj.GetType().GetField(fieldName, 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (T?)field?.GetValue(obj);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Helper method to set private field values using reflection
        /// </summary>
        private void SetPrivateField(object obj, string fieldName, object? value)
        {
            try
            {
                var field = obj.GetType().GetProperty(fieldName, 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                field?.SetValue(obj, value);
            }
            catch
            {
                // Silently fail if field can't be set
            }
        }

        #endregion

        public bool HasExistingData()
        {
            return dataPersistence.HasData();
        }

        public void ClearAllData()
        {
            dataPersistence.ClearAllData();
        }
    }
}
