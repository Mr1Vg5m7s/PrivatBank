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
        }

        public void DeletedExpense(int id)
        {
            Expense? expense = expenses.FirstOrDefault(i=> i.Id == id);
            if (expense != null)
            {
                expenses.Remove(expense);
            }
        }
        ///// категории

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
    }
}
