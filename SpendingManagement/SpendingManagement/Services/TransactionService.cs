using SpendingManagement.Models;
using SpendingManagement.Repositories;

namespace SpendingManagement.Services
{
    public class TransactionService
    {
        private TransactionRepository _transactionRepo;

        public TransactionService(TransactionRepository transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public void Add(Transaction transaction)
        {
            _transactionRepo.Add(transaction);
        }

        public Transaction GetTransactionById(int id)
        {
            return _transactionRepo.GetTransactionById(id);
        }

        public List<Transaction> GetTransactionsByWalletId(int walletId)
        {
            return _transactionRepo.GetTransactionsByWalletId(walletId);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _transactionRepo.UpdateTransaction(transaction);
        }

        public void DeleteTransaction(int id)
        {
            _transactionRepo.DeleteTransaction(id);
        }
        public List<Transaction> GetTransactionBetweenDates(int walletId, DateTime start, DateTime end, int userId)
        {
            return _transactionRepo.GetTransactionBetweenDates(walletId, start, end, userId);
        }
        public List<Transaction> GetRecentTransactions(int userId)
        {
            return _transactionRepo.GetRecentTransactions(userId);
        }
        public List<Transaction> GetTransactionsByFilter(int walletId, string filter, int userId)
        {
            return _transactionRepo.GetTransactionsByFilter(walletId, filter, userId);
        }
        public List<Transaction> GetTransactionsByMonth(int walletId, DateTime month, int userId)
        {
            return _transactionRepo.GetTransactionsByMonth(walletId, month, userId);
        }
    }
}
