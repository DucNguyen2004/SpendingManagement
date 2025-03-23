using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class IndexModel : PageModel
    {
        private WalletService _walletService;
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
            // Dummy data (replace with database logic)
            //Wallets = new List<Wallet>
            //{
            //    new Wallet { Id = 1, Name = "Cash", Balance = -203433000},
            //    new Wallet { Id = 2, Name = "Card", Balance = 7274000},
            //    new Wallet { Id = 3, Name = "VPS", Balance = 23000000},
            //    new Wallet { Id = 4, Name = "Home", Balance = -1355000}
            //};

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

        public void OnPostAsync()
        {

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
}
