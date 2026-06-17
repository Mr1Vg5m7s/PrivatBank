using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class Income
    {
        public int Id { get; set; } 
        private static int nextId = 1;
        public CategoryIncome? Category { get; set; }

        public Wallet? Wallet { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }

        public Income(CategoryIncome categoryIncome, Wallet wallet, decimal sum, DateTime date)
        {
            Id = nextId++;
            Category = categoryIncome;
            Wallet = wallet;
            Sum = sum;
            Date = date;
        }
    }
}
