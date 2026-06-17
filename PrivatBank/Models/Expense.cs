using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class Expense
    {
        public int Id { get; set; }
        private static int nextId = 1;
        public CategoryExpense? Category { get; set; }
        public Wallet? Wallet { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }

        public Expense(CategoryExpense categoryExpense, Wallet wallet, decimal sum, DateTime date)
        {
            Id = nextId++; //
            Category = categoryExpense;
            Wallet = wallet;
            Sum = sum;
            Date = date;
        }
    }
}
