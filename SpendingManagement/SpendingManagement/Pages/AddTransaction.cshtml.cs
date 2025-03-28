using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class AddTransactionModel : PageModel
    {
        private readonly CategoryService _categoryService;
        private readonly WalletService _walletService;
        private readonly TransactionService _transactionService;
        public AddTransactionModel(CategoryService categoryService, WalletService walletService, TransactionService transactionService)
        {
            _categoryService = categoryService;
            _walletService = walletService;
            _transactionService = transactionService;
        }
        [BindProperty]
        public Transaction Transaction { get; set; }

        public List<Wallet> Wallets { get; set; }
        public List<Category> IncomeCategories { get; set; }
        public List<Category> ExpenseCategories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null) return RedirectToPage("/Login");

            Wallets = _walletService.GetAll(userId.Value);
            if (Wallets.Count == 0)
            {
                return RedirectToPage("/Wallets");
            }
            IncomeCategories = _categoryService.GetIncomeCategories();
            ExpenseCategories = _categoryService.GetExpenseCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            _transactionService.Add(Transaction);

            return RedirectToPage("/Transactions");
        }

    }
}
