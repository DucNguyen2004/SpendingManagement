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
            var wallet = _context.Wallets.Find(transaction.WalletId);
            if (wallet == null) return;

            // Adjust wallet balance based on transaction type
            if (transaction.Type.ToLower().Equals("income"))
            {
                wallet.Balance += transaction.Amount;
            }
            else if (transaction.Type.ToLower().Equals("expense"))
            {
                wallet.Balance -= transaction.Amount;
            }
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
            if (existingTransaction == null) return;

            var wallet = _context.Wallets.Find(transaction.WalletId);
            if (wallet == null) return;

            // Revert the old transaction impact
            if (existingTransaction.Type == "income")
            {
                wallet.Balance -= existingTransaction.Amount;
            }
            else if (existingTransaction.Type == "expense")
            {
                wallet.Balance += existingTransaction.Amount;
            }

            // Apply the new transaction impact
            if (transaction.Type == "income")
            {
                wallet.Balance += transaction.Amount;
            }
            else if (transaction.Type == "expense")
            {
                wallet.Balance -= transaction.Amount;
            }

            // Update transaction details
            existingTransaction.WalletId = transaction.WalletId;
            existingTransaction.CategoryId = transaction.CategoryId;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Type = transaction.Type;
            existingTransaction.Note = transaction.Note;
            existingTransaction.Date = transaction.Date;

            _context.SaveChanges();
        }

        public void DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null) return;

            var wallet = _context.Wallets.Find(transaction.WalletId);
            if (wallet != null)
            {
                if (transaction.Type == "income")
                {
                    wallet.Balance -= transaction.Amount;
                }
                else if (transaction.Type == "expense")
                {
                    wallet.Balance += transaction.Amount;
                }
            }

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }

        public List<Transaction> GetTransactionBetweenDates(int walletId, DateTime start, DateTime end, int userId)
        {
            var baseQuery = _context.Transactions
                .Include(t => t.User).Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .Include(t => t.Wallet)
                .Where(t => t.UserId == userId && t.Date >= start && t.Date <= end);
            IQueryable<Transaction> query = baseQuery;

            if (walletId != 0)
            {
                query = query.Where(t => t.WalletId == walletId);
            }
            return query.OrderByDescending(t => t.Date).ToList();
        }

        public List<Transaction> GetRecentTransactions(int userId)
        {
            return _context.Transactions.Include(t => t.Category).Include(t => t.Wallet)
                .Where(t => t.UserId == userId && t.Date <= DateTime.Now)
                .OrderByDescending(t => t.Id) // Sort by newest
                .Take(3) // Show only the latest 5 transactions.ToList();
                .ToList();
        }

        public List<Transaction> GetTransactionsByFilter(int walletId, string filter, int userId)
        {
            var baseQuery = _context.Transactions
                .Include(t => t.User).Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .Include(t => t.Wallet);

            IQueryable<Transaction> query = baseQuery;

            if (walletId != 0)
            {
                query = query.Where(t => t.WalletId == walletId);
            }

            DateTime today = DateTime.UtcNow;
            switch (filter.ToLower())
            {
                case "day":
                    query = query.Where(t => t.Date.Date == today.Date);
                    break;
                case "week":
                    DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    query = query.Where(t => t.Date.Date >= startOfWeek);
                    break;
                case "month":
                    query = query.Where(t => t.Date.Year == today.Year && t.Date.Month == today.Month);
                    break;
                case "quarter":
                    int currentQuarter = (today.Month - 1) / 3 + 1;
                    query = query.Where(t => t.Date.Year == today.Year &&
                                             (t.Date.Month - 1) / 3 + 1 == currentQuarter);
                    break;
                case "year":
                    query = query.Where(t => t.Date.Year == today.Year);
                    break;
                case "all":
                    break; // No filtering
            }

            return query.OrderByDescending(t => t.Date).ToList();
        }

        public List<Transaction> GetTransactionsByMonth(int walletId, DateTime month, int userId)
        {
            var baseQuery = _context.Transactions
                .Include(t => t.User).Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .Include(t => t.Wallet)
                .Where(t => t.Date.Year == month.Year && t.Date.Month == month.Month);
            IQueryable<Transaction> query = baseQuery;

            if (walletId != 0)
            {
                query = query.Where(t => t.WalletId == walletId);
            }
            return query.OrderByDescending(t => t.Date).ToList();
            //return _context.Transactions
            //    .Include(t => t.User)
            //    .Include(t => t.Category)
            //    .Include(t => t.Wallet)
            //    .Where(t => t.WalletId == walletId && t.UserId == userId &&
            //                t.Date.Year == month.Year && t.Date.Month == month.Month)
            //    .OrderByDescending(t => t.Date)
            //    .ToList();
        }

    }

}
