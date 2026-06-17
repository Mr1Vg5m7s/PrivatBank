using System;
using System.Collections.Generic;
using System.Text;
using PrivatBank.Models;

namespace PrivatBank.Services
{
    internal class WalletService
    {
        private List<Wallet> wallets = new();

        public void AddWallet(Wallet wallet)
        {
            wallets.Add(wallet);
        }

        public void DeleteWallet(int id)
        {
            Wallet? wallet = wallets.FirstOrDefault(w => w.Id == id);
            if (wallet != null)
            {
                wallets.Remove(wallet);
            }
        }

        public Wallet? GetWallet(int id)
        {
            return wallets.FirstOrDefault(w => w.Id == id);
        }

        public List<Wallet> GetAllWallets()
        {
            return wallets;
        }

        public void UpdateWalletBalance(int walletId, decimal amount)
        {
            Wallet? wallet = GetWallet(walletId);
            if (wallet != null)
            {
                wallet.Balance += amount;
            }
        }

        public void SetWalletBalance(int walletId, decimal balance)
        {
            Wallet? wallet = GetWallet(walletId);
            if (wallet != null)
            {
                wallet.Balance = balance;
            }
        }
    }
}