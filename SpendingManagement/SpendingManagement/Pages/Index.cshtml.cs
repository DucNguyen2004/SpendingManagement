using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SpendingManagement.Models;
using SpendingManagement.Services;
using System.Globalization;

namespace SpendingManagement.Pages
{
    public class IndexModel : PageModel
    {
        private WalletService _walletService;
        private TransactionService _transactionService;

        [BindProperty]
        public string WalletName { get; set; }

        [BindProperty]
        public decimal WalletBalance { get; set; }
        public List<Wallet> Wallets { get; set; } = new();
        public List<TopSpendingItem> TopSpending { get; set; }
        public List<RecentTransactionItem> RecentTransactions { get; set; }
        public string SelectedFilter { get; set; }

        public string LabelsJson { get; set; } = "";
        public string ActualLabelsJson { get; set; } = "";
        public string ExpenseJson { get; set; } = "";
        public string ExpenseAvgJson { get; set; } = "";
        public string IncomeJson { get; set; } = "";
        public string IncomeAvgJson { get; set; } = "";
        public decimal ExpenseTotal { get; set; }
        public decimal IncomeTotal { get; set; }

        public List<TopSpendingItem> WeeklySpending { get; set; } = new();
        public List<TopSpendingItem> MonthlySpending { get; set; } = new();
        public string SelectedSpendingFilter { get; set; } = "Month"; // Default filter
        public IndexModel(WalletService walletService, TransactionService transactionService)
        {
            _walletService = walletService;
            _transactionService = transactionService;
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

            GetDataForChart();

            var now = DateTime.Now;
            var startOfWeek = now.AddDays(-((int)now.DayOfWeek)).Date; // Start of current week (Sunday)
            var startOfMonth = new DateTime(now.Year, now.Month, 1);   // Start of the month

            // Group & calculate for weekly and monthly data
            WeeklySpending = CalculateTopSpending(startOfWeek);
            MonthlySpending = CalculateTopSpending(startOfMonth);

            if (userId != null)
                GetRecentTransactions(userId.Value);
        }

        private void GetDataForChart()
        {
            var now = DateTime.Now;
            // Define the current month range
            DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Fetch transactions for the current month
            var transactions = _transactionService.GetTransactionBetweenDates(startOfMonth, endOfMonth);

            // Group transactions by date
            var groupedData = transactions
                .GroupBy(t => t.Date) // Group by day only
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd MMM"),
                    Expense = g.Where(t => t.Type == "expense").Sum(t => t.Amount),
                    Income = g.Where(t => t.Type == "income").Sum(t => t.Amount)
                })
                 .ToDictionary(t => t.Date, t => t);

            // Generate actual labels (every day of the month)
            var actualLabels = Enumerable.Range(0, (endOfMonth - startOfMonth).Days + 1)
                .Select(i => startOfMonth.AddDays(i).ToString("dd MMM"))
                .ToList();

            // Generate simplified labels (start and end only)
            var labels = new List<string>();
            for (int i = 0; i < actualLabels.Count; i++)
            {
                if (i == 0 || i == actualLabels.Count - 1)
                    labels.Add(actualLabels[i]);
                else labels.Add("");
            }

            // Cumulative expense and income sums
            var expenseData = new List<decimal>();
            var incomeData = new List<decimal>();
            decimal cumulativeExpense = 0, cumulativeIncome = 0;

            foreach (var label in actualLabels)
            {
                var date = DateTime.ParseExact(label, "dd MMM", CultureInfo.InvariantCulture);

                if (date > now) break;
                if (groupedData.ContainsKey(label))
                {
                    // If a transaction exists, update values
                    cumulativeExpense += groupedData[label].Expense;
                    cumulativeIncome += groupedData[label].Income;
                }

                // Store data
                expenseData.Add(cumulativeExpense);
                incomeData.Add(cumulativeIncome);
            }

            // Compute 3-month average data
            DateTime threeMonthsAgo = startOfMonth.AddMonths(-3);
            var pastTransactions = _transactionService.GetTransactionBetweenDates(threeMonthsAgo, startOfMonth);

            var pastGroupedData = pastTransactions
                .GroupBy(t => t.Date.Day)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Date = g.Key.ToString("dd MMM"),
                    Expense = g.Where(t => t.Type == "expense").Sum(t => t.Amount) / 3, // Divide by 3 months
                    Income = g.Where(t => t.Type == "income").Sum(t => t.Amount) / 3
                })
                .ToDictionary(t => t.Date, t => t);

            var expenseAvg = new List<decimal>();
            var incomeAvg = new List<decimal>();
            decimal cumulativeExpenseAvg = 0, cumulativeIncomeAvg = 0;

            foreach (var label in actualLabels)
            {
                var date = DateTime.ParseExact(label, "dd MMM", CultureInfo.InvariantCulture);

                if (date > now) break;
                if (pastGroupedData.ContainsKey(label))
                {
                    // If a transaction exists, update values
                    cumulativeExpenseAvg += pastGroupedData[label].Expense;
                    cumulativeIncomeAvg += pastGroupedData[label].Income;
                }

                // Store data
                expenseAvg.Add(cumulativeExpenseAvg);
                incomeAvg.Add(cumulativeIncomeAvg);
            }

            // Calculate totals
            ExpenseTotal = transactions.Where(t => t.Type == "expense").Sum(t => t.Amount);
            IncomeTotal = transactions.Where(t => t.Type == "income").Sum(t => t.Amount);

            // Convert to JSON for JavaScript
            LabelsJson = JsonConvert.SerializeObject(labels);
            ActualLabelsJson = JsonConvert.SerializeObject(actualLabels);
            ExpenseJson = JsonConvert.SerializeObject(expenseData);
            ExpenseAvgJson = JsonConvert.SerializeObject(expenseAvg);
            IncomeJson = JsonConvert.SerializeObject(incomeData);
            IncomeAvgJson = JsonConvert.SerializeObject(incomeAvg);
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

        private List<TopSpendingItem> CalculateTopSpending(DateTime startDate)
        {
            var filteredTransactions = _transactionService.GetTransactionBetweenDates(startDate, DateTime.Now).Where(t => t.Type.Equals("expense"));
            var totalExpense = filteredTransactions.Sum(t => (decimal?)t.Amount) ?? 0;

            return filteredTransactions
                .GroupBy(t => t.CategoryId)
                .Select(g => new TopSpendingItem
                {
                    Category = g.First().Category.Name,
                    //Icon = g.First().Category.Icon,
                    Amount = g.Sum(t => t.Amount),
                    Percentage = totalExpense > 0 ? (int)Math.Round((g.Sum(t => t.Amount) / totalExpense) * 100, 2) : 0
                })
                .OrderByDescending(x => x.Amount)
                .Take(3) // Show Top 3
                .ToList();
        }

        private void GetRecentTransactions(int userId)
        {
            // Load the 5 most recent transactions
            RecentTransactions = _transactionService.GetRecentTransactions(userId)
                .Select(t => new RecentTransactionItem
                {
                    Category = t.Category.Name,
                    Icon = t.Category.Icon,
                    Amount = t.Amount,
                    Date = t.Date
                })
                .ToList();
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

