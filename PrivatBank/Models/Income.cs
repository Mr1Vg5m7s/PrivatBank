using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class Income
    {
        public int Id { get; set; } //+- В
        private static int nextId = 1;
        public CategoryIncome? Catogory { get; set; }
        public int Sum { get; set; }
        public DateTime Date { get; set; }

        public Income(CategoryIncome categoryIncome, int sum, DateTime date)
        {
            Id = nextId++; // 
            Catogory = categoryIncome;
            Sum = sum;
            Date = date;
        }
    }
}
