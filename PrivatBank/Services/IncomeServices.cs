using System;
using System.Collections.Generic;
using System.Text;
using PrivatBank.Models;

namespace PrivatBank.Services
{
    internal class IncomeServices
    {
        private List<Income> incomes = new List<Income>(); 
        private List<CategoryIncome> categories = new();

        public void AddIncome(Income income)
        {
            incomes.Add(income);
            if (income.Wallet != null)
            {
                income.Wallet.Balance += income.Sum;
            }
        }

        public void DeleteIncome(int id)
        {
            Income? income = incomes.FirstOrDefault(i => i.Id == id);

            if (income != null)
            {
                if (income.Wallet != null)
                {
                    income.Wallet.Balance -= income.Sum;
                }
                incomes.Remove(income);
            }
        }
        //доходы
        public Income? GetIncome(int id)
        {
            return incomes.FirstOrDefault(i => i.Id == id);
        }

        public List<Income> GetAllIncomes()
        {
            return incomes;
        }

        public List<Income> GetIncomesByWallet(int walletId)
        {
            return incomes.Where(i => i.Wallet?.Id == walletId).ToList();
        }
        ///// категории
        public void AddCategoryIncome(CategoryIncome categoryIncome)
        {
            categories.Add(categoryIncome);
        }

        public void DeleteCategory(int id)
        {
            CategoryIncome? category = categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                categories.Remove(category);
            }
        }
        //
        public CategoryIncome? GetCategoryIncome(int id)
        {
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public List<CategoryIncome> GetAllCategoriesIncome()
        {
            return categories;
        }
    }
}
