using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class IndexModel : PageModel
    {
        private WalletService _walletService;

        [BindProperty]
        public string WalletName { get; set; }

        [BindProperty]
        public decimal WalletBalance { get; set; }
        public List<Wallet> Wallets { get; set; } = new();
        public List<TopSpendingItem> TopSpending { get; set; }
        public List<RecentTransactionItem> RecentTransactions { get; set; }
        public string SelectedFilter { get; set; }

        public IndexModel(WalletService walletService)
        {
            _walletService = walletService;
        }
        public void OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                Wallets = _walletService.GetAll(userId.Value);
            }

            // Get top 3 included wallets
            Wallets = Wallets
                             .Take(3)
                             .ToList();

            TopSpending = new List<TopSpendingItem>
            {
                new TopSpendingItem { Category = "Bills & Utilities", Amount = 5459000, Percentage = 19, Icon = "/images/bills.png" },
                new TopSpendingItem { Category = "Investment", Amount = 4800000, Percentage = 17, Icon = "/images/investment.png" },
                new TopSpendingItem { Category = "Health & Fitness", Amount = 3870000, Percentage = 13, Icon = "/images/health.png" }
            };

            RecentTransactions = new List<RecentTransactionItem>
            {
                new RecentTransactionItem { Category = "Food & Beverage", Amount = -300000, Date = DateTime.Parse("2024-06-26"), Icon = "/images/food.png" },
                new RecentTransactionItem { Category = "Shopping", Amount = -110000, Date = DateTime.Parse("2024-06-26"), Icon = "/images/shopping.png" },
                new RecentTransactionItem { Category = "Restaurants", Amount = -10000, Date = DateTime.Parse("2024-06-26"), Icon = "/images/restaurant.png" }
            };
        }

        public IActionResult OnPostAddWalletAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Wallet newWallet = new Wallet
            {
                UserId = userId.Value,
                Name = WalletName,
                Balance = WalletBalance
            };

            _walletService.AddWallet(newWallet);

            return RedirectToPage();
        }
    }
}
public class TopSpendingItem
{
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public int Percentage { get; set; }
    public string Icon { get; set; }
}

public class RecentTransactionItem
{
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Icon { get; set; }
}

