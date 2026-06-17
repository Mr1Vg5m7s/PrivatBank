using System;
using System.Collections.Generic;
using System.Text;

namespace PrivatBank.Models
{
    internal class Wallet
    {
        public int Id { get; set; }
        private static int nextId = 1;
        public string? Name { get; set; }
        public Currency? Currency { get; set; }
        public decimal Balance { get; set; }

        public Wallet(string name, Currency currency, decimal balance = 0)
        {
            Id = nextId++;
            Name = name;
            Currency = currency;
            Balance = balance;
        }
    }
}