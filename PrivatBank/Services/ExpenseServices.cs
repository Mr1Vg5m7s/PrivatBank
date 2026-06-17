using System;
using System.Collections.Generic;
using System.Text;
using PrivatBank.Models;

namespace PrivatBank.Services
{
    internal class ExpenseServices
    {
        private List<Expense> expenses = new();
        private List<CategoryExpense> categories = new();

        public void AddExpense(Expense expense)
        {
            expenses.Add(expense);
            if (expense.Wallet != null)
            {
                expense.Wallet.Balance -= expense.Sum;
            }
        }

        public void DeletedExpense(int id)
        {
            Expense? expense = expenses.FirstOrDefault(i=> i.Id == id);
            if (expense != null)
            {
                if (expense.Wallet != null)
                {
                    expense.Wallet.Balance += expense.Sum;
                }
                expenses.Remove(expense);
            }
        }
        ///////
        public Expense? GetExpense(int id)
        {
            return expenses.FirstOrDefault(e => e.Id == id);
        }

        public List<Expense> GetAllExpenses()
        {
            return expenses;
        }

        public List<Expense> GetExpensesByWallet(int walletId)
        {
            return expenses.Where(e => e.Wallet?.Id == walletId).ToList();
        }
        /////категории
        public void AddCategoryExpense(CategoryExpense categoryExpense)
        {
            CategoryExpense category = categoryExpense;
        }

        
        public void DeleteCategoryExpense(int id)
        {
            CategoryExpense? category = categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                categories.Remove(category);
            }
        }

        ////
        public CategoryExpense? GetCategoryExpense(int id)
        {
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public List<CategoryExpense> GetAllCategoriesExpense()
        {
            return categories;
        }
    }
}
