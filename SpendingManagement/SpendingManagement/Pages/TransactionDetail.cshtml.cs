using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class TransactionDetailModel : PageModel
    {
        private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;
        private readonly WalletService _walletService;

        public TransactionDetailModel(TransactionService transactionService, CategoryService categoryService, WalletService walletService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            _walletService = walletService;
        }

        [BindProperty]
        public Transaction Transaction { get; set; }
        public List<Wallet> Wallets { get; set; }
        public List<Category> IncomeCategories { get; set; }
        public List<Category> ExpenseCategories { get; set; }
        public IActionResult OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            Transaction = _transactionService.GetTransactionById(id);
            Wallets = _walletService.GetAll(userId.Value);
            IncomeCategories = _categoryService.GetIncomeCategories();
            ExpenseCategories = _categoryService.GetExpenseCategories();
            if (Transaction == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostEdit()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Set Transaction Type Based on Category Type
            var selectedCategory = _categoryService.GetCategory(Transaction.CategoryId);
            if (selectedCategory == null || Transaction.WalletId == 0)
            {
                Wallets = _walletService.GetAll(userId.Value);
                IncomeCategories = _categoryService.GetIncomeCategories();
                ExpenseCategories = _categoryService.GetExpenseCategories();
                //ModelState.AddModelError("Transaction.CategoryId", "Invalid category.");
                return Page();
            }

            Transaction.Type = selectedCategory.Type;
            Transaction.UserId = userId.Value;

            _transactionService.UpdateTransaction(Transaction);
            return RedirectToPage("/Transactions");
        }

        public IActionResult OnPostDelete(int id)
        {
            var transaction = _transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _transactionService.DeleteTransaction(id);
            return RedirectToPage("/Transactions");
        }
    }
}
