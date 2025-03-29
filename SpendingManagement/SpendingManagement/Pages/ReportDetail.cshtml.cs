using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class ReportDetailModel : PageModel
    {
        private TransactionService transactionService;
        public ReportDetailModel(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new();
        public string ExpenseChartData { get; set; }
        public string IncomeChartData { get; set; }
        public string NetIncomeChartData { get; set; }
        public string SelectedFilter { get; set; } = "Month"; // Default filter
        public string SelectedMonth { get; set; }

        public IActionResult OnGet(string filter = null)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("Login");
            }
            // Default to current month if no filter is provided
            var now = DateTime.Now;
            if (string.IsNullOrEmpty(filter))
            {
                filter = now.ToString("yyyy-MM");
            }

            SelectedFilter = filter;

            var startOfMonth = new DateTime(now.Year, now.Month, 1);   // Start of the month
            int walletId = 1; // Replace with actual wallet selection logic


            // Convert the selected filter (YYYY-MM) to DateTime
            DateTime selectedMonth;
            if (!DateTime.TryParseExact(filter, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out selectedMonth))
            {
                selectedMonth = now;
            }
            List<Transaction> transactions;
            transactions = transactionService.GetTransactionsByMonth(walletId, selectedMonth, userId.Value);

            Transactions = transactions
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Category = t.Category?.Name ?? "Uncategorized",
                    Description = t.Note,
                    Amount = t.Amount,
                    Type = t.Type
                }).ToList();

            TotalIncome = Transactions.Where(t => t.Type.ToLower() == "income").Sum(t => t.Amount);
            TotalExpense = Transactions.Where(t => t.Type.ToLower() == "expense").Sum(t => t.Amount);
            Balance = TotalIncome - TotalExpense;

            var expenseData = Transactions.Where(t => t.Type.ToLower() == "expense")
                .GroupBy(t => t.Category)
                .Select(g => new { Label = g.Key, Value = g.Sum(t => t.Amount) })
                .ToList();

            var incomeData = Transactions.Where(t => t.Type.ToLower() == "income")
                .GroupBy(t => t.Category)
                .Select(g => new { Label = g.Key, Value = g.Sum(t => t.Amount) })
                .ToList();

            // Group transactions by week for Net Income Chart
            var firstDayOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var allWeeks = new List<(DateTime Start, DateTime End, string WeekLabel)>();

            DateTime start = firstDayOfMonth;

            // Find the first Sunday of the month (end of first partial week)
            DateTime firstSunday = start.AddDays(7 - (int)start.DayOfWeek);
            if (firstSunday > lastDayOfMonth) firstSunday = lastDayOfMonth;

            // First week (may not be full)
            allWeeks.Add((start, firstSunday, $"{start.Day}-{firstSunday.Day}"));
            start = firstSunday.AddDays(1); // Move to next Monday

            // Full Monday-Sunday weeks
            while (start.AddDays(6) <= lastDayOfMonth)
            {
                DateTime end = start.AddDays(6); // Ends on Sunday
                allWeeks.Add((start, end, $"{start.Day}-{end.Day}"));
                start = end.AddDays(1); // Move to next Monday
            }

            // Last week (if not full, adjust end to last day of month)
            if (start <= lastDayOfMonth)
            {
                allWeeks.Add((start, lastDayOfMonth, $"{start.Day}-{lastDayOfMonth.Day}"));
            }

            // Group transactions by week range
            var transactionGroups = Transactions
                .GroupBy(t => allWeeks.FirstOrDefault(w => t.Date >= w.Start && t.Date <= w.End).WeekLabel)
                .Select(g => new
                {
                    WeekLabel = g.Key,
                    Income = g.Where(t => t.Type.ToLower() == "income").Sum(t => t.Amount),
                    Expense = g.Where(t => t.Type.ToLower() == "expense").Sum(t => t.Amount) * -1 // Make expenses negative
                })
                .ToList();

            // Merge transaction data with allWeeks (fill missing weeks with 0)
            var netIncomeData = allWeeks
                .GroupJoin(transactionGroups, w => w.WeekLabel, t => t.WeekLabel,
                    (w, t) => new
                    {
                        w.WeekLabel,
                        Income = t.Sum(x => x.Income),
                        Expense = t.Sum(x => x.Expense)
                    })
                //.OrderBy(w => w.WeekLabel)
                .ToList();


            // Convert to JSON for Chart.js
            ExpenseChartData = JsonConvert.SerializeObject(expenseData);
            IncomeChartData = JsonConvert.SerializeObject(incomeData);
            NetIncomeChartData = JsonConvert.SerializeObject(netIncomeData);

            return Page();
        }
    }
}
