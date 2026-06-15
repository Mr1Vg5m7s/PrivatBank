using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class CategoryExpense
    {
        public int Id { get; set; }
        private static int nextId = 1;
        public string? Name { get; set; }

        public CategoryExpense(int id, string name)
        {
            Id = nextId++;
            Name = name;
        }
    }
}
