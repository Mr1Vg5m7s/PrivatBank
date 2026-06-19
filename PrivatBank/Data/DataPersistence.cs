using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using PrivatBank.Models;
using PrivatBank.Services;

namespace PrivatBank.Data
{
    internal class DataPersistence
    {
        private readonly string dataDirectory;
        private readonly string walletsFile;
        private readonly string expensesFile;
        private readonly string incomesFile;
        private readonly string expenseCategoriesFile;
        private readonly string incomeCategoriesFile;
        private readonly string currenciesFile;

        private readonly JsonSerializerOptions jsonOptions;

        public DataPersistence(string baseDirectory = "Data")
        {
            dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, baseDirectory);

            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            walletsFile = Path.Combine(dataDirectory, "wallets.json");
            expensesFile = Path.Combine(dataDirectory, "expenses.json");
            incomesFile = Path.Combine(dataDirectory, "incomes.json");
            expenseCategoriesFile = Path.Combine(dataDirectory, "expenseCategories.json");
            incomeCategoriesFile = Path.Combine(dataDirectory, "incomeCategories.json");
            currenciesFile = Path.Combine(dataDirectory, "currencies.json");

            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
                
        public void SaveWallets(List<Wallet> wallets)
        {
            try
            {
                var json = JsonSerializer.Serialize(wallets, jsonOptions);
                File.WriteAllText(walletsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving wallets: {ex.Message}");
            }
        }

        public void SaveExpenses(List<Expense> expenses)
        {
            try
            {
                var data = new List<ExpenseData>();
                foreach (var expense in expenses)
                {
                    data.Add(new ExpenseData
                    {
                        Id = expense.Id,
                        CategoryId = expense.Category?.Id ?? 0,
                        WalletId = expense.Wallet?.Id ?? 0,
                        Sum = expense.Sum,
                        Date = expense.Date
                    });
                }

                var json = JsonSerializer.Serialize(data, jsonOptions);
                File.WriteAllText(expensesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expenses: {ex.Message}");
            }
        }

        public void SaveIncomes(List<Income> incomes)
        {
            try
            {
                var data = new List<IncomeData>();
                foreach (var income in incomes)
                {
                    data.Add(new IncomeData
                    {
                        Id = income.Id,
                        CategoryId = income.Category?.Id ?? 0,
                        WalletId = income.Wallet?.Id ?? 0,
                        Sum = income.Sum,
                        Date = income.Date
                    });
                }

                var json = JsonSerializer.Serialize(data, jsonOptions);
                File.WriteAllText(incomesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving incomes: {ex.Message}");
            }
        }

        public void SaveExpenseCategories(List<CategoryExpense> categories)
        {
            try
            {
                var json = JsonSerializer.Serialize(categories, jsonOptions);
                File.WriteAllText(expenseCategoriesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expense categories: {ex.Message}");
            }
        }

        public void SaveIncomeCategories(List<CategoryIncome> categories)
        {
            try
            {
                var json = JsonSerializer.Serialize(categories, jsonOptions);
                File.WriteAllText(incomeCategoriesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving income categories: {ex.Message}");
            }
        }

        public void SaveCurrencies(List<Currency> currencies)
        {
            try
            {
                var json = JsonSerializer.Serialize(currencies, jsonOptions);
                File.WriteAllText(currenciesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving currencies: {ex.Message}");
            }
        }
                
        public List<Wallet> LoadWallets()
        {
            try
            {
                if (!File.Exists(walletsFile))
                    return new List<Wallet>();

                var json = File.ReadAllText(walletsFile);
                return JsonSerializer.Deserialize<List<Wallet>>(json, jsonOptions) ?? new List<Wallet>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading wallets: {ex.Message}");
                return new List<Wallet>();
            }
        }

        public List<ExpenseData> LoadExpenses()
        {
            try
            {
                if (!File.Exists(expensesFile))
                    return new List<ExpenseData>();

                var json = File.ReadAllText(expensesFile);
                return JsonSerializer.Deserialize<List<ExpenseData>>(json, jsonOptions) ?? new List<ExpenseData>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expenses: {ex.Message}");
                return new List<ExpenseData>();
            }
        }

        public List<IncomeData> LoadIncomes()
        {
            try
            {
                if (!File.Exists(incomesFile))
                    return new List<IncomeData>();

                var json = File.ReadAllText(incomesFile);
                return JsonSerializer.Deserialize<List<IncomeData>>(json, jsonOptions) ?? new List<IncomeData>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading incomes: {ex.Message}");
                return new List<IncomeData>();
            }
        }

        public List<CategoryExpense> LoadExpenseCategories()
        {
            try
            {
                if (!File.Exists(expenseCategoriesFile))
                    return new List<CategoryExpense>();

                var json = File.ReadAllText(expenseCategoriesFile);
                return JsonSerializer.Deserialize<List<CategoryExpense>>(json, jsonOptions) ?? new List<CategoryExpense>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expense categories: {ex.Message}");
                return new List<CategoryExpense>();
            }
        }

        public List<CategoryIncome> LoadIncomeCategories()
        {
            try
            {
                if (!File.Exists(incomeCategoriesFile))
                    return new List<CategoryIncome>();

                var json = File.ReadAllText(incomeCategoriesFile);
                return JsonSerializer.Deserialize<List<CategoryIncome>>(json, jsonOptions) ?? new List<CategoryIncome>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading income categories: {ex.Message}");
                return new List<CategoryIncome>();
            }
        }

        public List<Currency> LoadCurrencies()
        {
            try
            {
                if (!File.Exists(currenciesFile))
                    return new List<Currency>();

                var json = File.ReadAllText(currenciesFile);
                return JsonSerializer.Deserialize<List<Currency>>(json, jsonOptions) ?? new List<Currency>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currencies: {ex.Message}");
                return new List<Currency>();
            }
        }
               
        public class ExpenseData
        {
            public int Id { get; set; }
            public int CategoryId { get; set; }
            public int WalletId { get; set; }
            public decimal Sum { get; set; }
            public DateTime Date { get; set; }
        }

        public class IncomeData
        {
            public int Id { get; set; }
            public int CategoryId { get; set; }
            public int WalletId { get; set; }
            public decimal Sum { get; set; }
            public DateTime Date { get; set; }
        }

        public bool HasData()
        {
            return File.Exists(walletsFile) || File.Exists(expensesFile) || 
                   File.Exists(incomesFile) || File.Exists(currenciesFile);
        }

        public void ClearAllData()
        {
            try
            {
                if (File.Exists(walletsFile)) File.Delete(walletsFile);
                if (File.Exists(expensesFile)) File.Delete(expensesFile);
                if (File.Exists(incomesFile)) File.Delete(incomesFile);
                if (File.Exists(expenseCategoriesFile)) File.Delete(expenseCategoriesFile);
                if (File.Exists(incomeCategoriesFile)) File.Delete(incomeCategoriesFile);
                if (File.Exists(currenciesFile)) File.Delete(currenciesFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing data: {ex.Message}");
            }
        }
    }
}
