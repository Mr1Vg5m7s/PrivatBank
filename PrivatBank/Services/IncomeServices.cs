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
        }

        public void DeleteIncome(int id)
        {
            Income? income = incomes.FirstOrDefault(i => i.Id == id);

            if (income != null)
            {
                incomes.Remove(income);
            }
        }
        ///// категории
        public void AddCategoryIncome(CategoryIncome categoryIncome)
        {
            CategoryIncome category = categoryIncome;
        }

        public void DeleteCategory(int id)
        {
            CategoryIncome? category = categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                categories.Remove(category);
            }
        }

    }
}
