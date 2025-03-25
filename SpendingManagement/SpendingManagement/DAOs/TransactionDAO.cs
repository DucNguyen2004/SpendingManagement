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

        //public List<Transaction> GetByWalletId(int walletId)
        //{
        //    return _context.Transactions.Where(t => t.WalletId == walletId).ToList();
        //}

        public Transaction GetById(int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id);
        }

        public void Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public void Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }

        public void Delete(int id)
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
