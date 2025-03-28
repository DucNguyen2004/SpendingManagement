using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class TransactionsModel : PageModel
    {
        private TransactionService transactionService;
        public List<TransactionGroupDto> TransactionsByDate { get; set; }
        public List<TransactionCategoryGroupDto> TransactionsByCategory { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public string SelectedFilter { get; set; } = "Month"; // Default filter

        public TransactionsModel(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }
        public IActionResult OnGet(string filter = "Month")
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            SelectedFilter = filter;
            int walletId = 1; // Replace with actual wallet selection logic

            var transactions = transactionService.GetTransactionsByFilter(walletId, filter, userId.Value);


            TransactionsByDate = GetTransactionsByDate(transactions);
            TransactionsByCategory = GetTransactionsByCategory(transactions);

            TotalIncome = transactions.Sum(t => t.Type.Equals("income") ? t.Amount : 0);
            TotalExpense = transactions.Sum(t => t.Type.Equals("expense") ? t.Amount : 0);

            return Page();
        }

        private List<TransactionGroupDto> GetTransactionsByDate(List<Transaction> listTransactions)
        {
            return listTransactions
                .GroupBy(t => t.Date.Date)
                .Select(g => new TransactionGroupDto
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(t => t.Type.Equals("income") ? t.Amount : -t.Amount),
                    Transactions = g.Select(t => new TransactionDto
                    {
                        Id = t.Id,
                        Date = t.Date,
                        Category = t.Category?.Name ?? "Uncategorized",
                        Description = t.Note,
                        Amount = t.Amount,
                        Type = t.Type.Equals("income") ? "text-success" : "text-danger"
                    }).ToList()
                })
                .ToList();
        }

        private List<TransactionCategoryGroupDto> GetTransactionsByCategory(List<Transaction> listTransactions)
        {
            var transactions = GetTransactionsByDate(listTransactions).SelectMany(g => g.Transactions).ToList();

            return transactions
                .GroupBy(t => t.Category)
                .Select(g => new TransactionCategoryGroupDto
                {
                    Category = g.Key,
                    TotalTransactions = g.Count(),
                    TotalAmount = g.Sum(t => t.Amount),
                    Transactions = g.ToList()
                })
                .ToList();
        }
    }
    public class TransactionGroupDto
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }

    public class TransactionCategoryGroupDto
    {
        public string Category { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalTransactions { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }

    public class TransactionDto
    {
        public int Id { get; set; }  // Add ID for detail link
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}
