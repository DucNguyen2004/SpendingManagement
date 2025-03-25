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

        public Transaction GetById(int id)
        {
            return _transactionDAO.GetById(id);
        }

        public void Add(Transaction transaction)
        {
            _transactionDAO.Add(transaction);
        }
        public void Delete(Transaction transaction)
        {
            _transactionDAO.Delete(transaction.Id);
        }
        public void Update(Transaction transaction)
        {
            _transactionDAO.Update(transaction);
        }
    }
}
