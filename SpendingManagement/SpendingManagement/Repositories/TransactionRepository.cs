using SpendingManagement.DAOs;
using SpendingManagement.Models;

namespace SpendingManagement.Repositories
{
    public class TransactionRepository
    {
        private TransactionDAO _transactionDAO;
        public TransactionRepository(TransactionDAO transactionDAO)
        {
            _transactionDAO = transactionDAO;
        }

        public void Add(Transaction transaction)
        {
            _transactionDAO.Add(transaction);
        }

        public Transaction GetTransactionById(int id)
        {
            return _transactionDAO.GetTransactionById(id);
        }

        public List<Transaction> GetTransactionsByWalletId(int walletId)
        {
            return _transactionDAO.GetTransactionsByWalletId(walletId);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _transactionDAO.UpdateTransaction(transaction);
        }

        public void DeleteTransaction(int id)
        {
            _transactionDAO.DeleteTransaction(id);
        }
        public List<Transaction> GetTransactionBetweenDates(int walletId, DateTime start, DateTime end, int userId)
        {
            return _transactionDAO.GetTransactionBetweenDates(walletId, start, end, userId);
        }
        public List<Transaction> GetRecentTransactions(int userId)
        {
            return _transactionDAO.GetRecentTransactions(userId);
        }

        public List<Transaction> GetTransactionsByFilter(int walletId, string filter, int userId)
        {
            return _transactionDAO.GetTransactionsByFilter(walletId, filter, userId);
        }
        public List<Transaction> GetTransactionsByMonth(int walletId, DateTime month, int userId)
        {
            return _transactionDAO.GetTransactionsByMonth(walletId, month, userId);
        }
    }
}
