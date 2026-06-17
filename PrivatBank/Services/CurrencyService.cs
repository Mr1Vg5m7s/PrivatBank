using System;
using System.Collections.Generic;
using System.Text;
using PrivatBank.Models;

namespace PrivatBank.Services
{
    internal class CurrencyService
    {
        private List<Currency> currencies = new();

        public void AddCurrency(Currency currency)
        {
            currencies.Add(currency);
        }

        public void DeleteCurrency(int id)
        {
            Currency? currency = currencies.FirstOrDefault(c => c.Id == id);
            if (currency != null)
            {
                currencies.Remove(currency);
            }
        }

        public Currency? GetCurrency(int id)
        {
            return currencies.FirstOrDefault(c => c.Id == id);
        }

        public List<Currency> GetAllCurrencies()
        {
            return currencies;
        }
    }
}