using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class Expense
    {
        public int Id { get; set; }// +- В
        private static int nextId = 1;
        public CategoryExpense? Category { get; set; }
        public int Sum { get; set; }
        public DateTime Date { get; set; }

        public Expense(CategoryExpense categoryExpense, int sum, DateTime date)
        {
            Id = nextId++; //
            Category = categoryExpense;
            Sum = sum;
            Date = date;
        }
    }
}
