using Microsoft.EntityFrameworkCore;
using SpendingManagement.Models;

namespace SpendingManagement.DAOs
{
    public class WalletDAO
    {
        private SpendingManagementContext context;
        public WalletDAO(SpendingManagementContext context)
        {
            this.context = context;
        }

        public List<Wallet> GetAll(int userId)
        {
            return context.Wallets.Include(w => w.User).Where(w => w.UserId == userId).ToList();
        }
        public Wallet GetWalletById(int walletId, int userId)
        {
            return context.Wallets.FirstOrDefault(w => w.Id == walletId && w.UserId == userId);
        }

        public void AddWallet(Wallet wallet)
        {
            context.Wallets.Add(wallet);
            context.SaveChanges();
        }

        // Update an existing wallet
        public bool UpdateWallet(Wallet updatedWallet, int userId)
        {
            var wallet = context.Wallets.FirstOrDefault(w => w.Id == updatedWallet.Id && w.UserId == userId);
            if (wallet == null)
            {
                return false; // Wallet not found or does not belong to the user
            }

            // Update properties
            wallet.Name = updatedWallet.Name;
            wallet.Balance = updatedWallet.Balance;

            context.SaveChanges();
            return true; // Successfully updated
        }

        // Delete a wallet by ID and user ID
        public bool DeleteWallet(int walletId, int userId)
        {
            var wallet = context.Wallets.FirstOrDefault(w => w.Id == walletId && w.UserId == userId);
            if (wallet == null)
            {
                return false; // Wallet not found or does not belong to the user
            }

            context.Wallets.Remove(wallet);
            context.SaveChanges();
            return true; // Successfully deleted
        }
    }
}
