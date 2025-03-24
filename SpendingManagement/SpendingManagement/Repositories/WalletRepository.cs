using SpendingManagement.DAOs;
using SpendingManagement.Models;

namespace SpendingManagement.Repositories
{
    public class WalletRepository
    {
        private WalletDAO _walletDAO;

        public WalletRepository(WalletDAO walletDAO)
        {
            _walletDAO = walletDAO;
        }

        public List<Wallet> GetAll(int userId)
        {
            return _walletDAO.GetAll(userId);
        }

        public void AddWallet(Wallet wallet)
        {
            _walletDAO.AddWallet(wallet);
        }

        public bool UpdateWallet(Wallet updatedWallet, int userId)
        {
            return _walletDAO.UpdateWallet(updatedWallet, userId);
        }

        public bool DeleteWallet(int walletId, int userId)
        {
            return _walletDAO.DeleteWallet(walletId, userId);
        }
        public Wallet GetWalletById(int walletId, int userId)
        {
            return _walletDAO.GetWalletById(walletId, userId);
        }
    }
}
