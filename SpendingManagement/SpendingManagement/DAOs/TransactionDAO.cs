using Microsoft.EntityFrameworkCore;
using SpendingManagement.Models;

namespace SpendingManagement.DAOs
{
    public class TransactionDAO
    {
        private readonly SpendingManagementContext _context;

        public TransactionDAO(SpendingManagementContext context)
        {
            _context = context;
        }

        public void Add(Transaction transaction)
        {
            _context.Add(transaction);
            _context.SaveChanges();
        }

        public Transaction GetTransactionById(int id)
        {
            return _context.Transactions.Include(t => t.Wallet)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<Transaction> GetTransactionsByWalletId(int walletId)
        {
            return _context.Transactions
                .Where(t => t.WalletId == walletId)
                .ToList();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var existingTransaction = _context.Transactions.Find(transaction.Id);
            if (existingTransaction != null)
            {
                existingTransaction.WalletId = transaction.WalletId;
                existingTransaction.CategoryId = transaction.CategoryId;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Type = transaction.Type;
                existingTransaction.Note = transaction.Note;
                existingTransaction.Date = transaction.Date;

                _context.SaveChanges();
            }
        }

        public void DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
    }

}
