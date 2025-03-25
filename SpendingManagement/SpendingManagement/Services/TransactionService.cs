using SpendingManagement.Models;
using SpendingManagement.Repositories;

namespace SpendingManagement.Services
{
    public class TransactionService
    {
        private TransactionRepository _transactionRepo;
        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepo = transactionRepository;
        }
        public Transaction GetById(int id)
        {
            return _transactionRepo.GetById(id);
        }
        public void Add(Transaction transaction)
        {
            _transactionRepo.Add(transaction);
        }
        public void Delete(Transaction transaction)
        {
            _transactionRepo.Delete(transaction);
        }
        public void Update(Transaction transaction)
        {
            _transactionRepo.Update(transaction);
        }
    }
}
