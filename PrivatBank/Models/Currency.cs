using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class Currency
    {
        public int Id { get; set; }
        private static int nextId = 1;
        public string? Code { get; set; } // USD, EUR, UAH и т.д.
        public string? Name { get; set; }
        public string? Symbol { get; set; }

        public Currency(string code, string name, string symbol)
        {
            Id = nextId++;
            Code = code;
            Name = name;
            Symbol = symbol;
        }
    }
}