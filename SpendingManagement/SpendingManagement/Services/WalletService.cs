using SpendingManagement.Models;
using SpendingManagement.Repositories;

namespace SpendingManagement.Services
{
    public class WalletService
    {
        private WalletRepository _walletRepo;

        public WalletService(WalletRepository walletRepository)
        {
            _walletRepo = walletRepository;
        }

        public List<Wallet> GetAll(int userId)
        {
            return _walletRepo.GetAll(userId);
        }

        public void AddWallet(Wallet wallet)
        {
            _walletRepo.AddWallet(wallet);
        }

        public bool UpdateWallet(Wallet updatedWallet, int userId)
        {
            return _walletRepo.UpdateWallet(updatedWallet, userId);
        }

        public bool DeleteWallet(int walletId, int userId)
        {
            return _walletRepo.DeleteWallet(walletId, userId);
        }
        public Wallet GetWalletById(int walletId, int userId)
        {
            return _walletRepo.GetWalletById(walletId, userId);
        }
    }
}
