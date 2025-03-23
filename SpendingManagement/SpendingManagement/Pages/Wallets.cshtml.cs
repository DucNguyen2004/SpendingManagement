using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;

namespace SpendingManagement.Pages
{
    public class WalletsModel : PageModel
    {
        private readonly SpendingManagementContext _context;

        public WalletsModel(SpendingManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string WalletName { get; set; }

        [BindProperty]
        public decimal WalletBalance { get; set; }
        public List<Wallet> Wallets { get; set; } = new();
        public decimal TotalBalance { get; set; }
        public void OnGet()
        {
            // Dummy Data
            Wallets = new List<Wallet>
            {
            new Wallet { Id = 1, Name = "Cash", Balance = -203433000 },
            new Wallet { Id = 2, Name = "Card", Balance = 7274000 },
            new Wallet { Id = 3, Name = "VPS", Balance = 23000000 }
            };

            // Calculate total balance
            TotalBalance = Wallets.Sum(w => w.Balance ?? 0);
        }

        //public async Task<IActionResult> OnGetAsync()
        //{
        //    Wallets = await _context.Wallets
        //        .OrderBy(w => w.DisplayOrder)
        //        .ToListAsync();
        //    return Page();
        //}

        //public async Task<IActionResult> OnPostReorderAsync([FromBody] List<int> walletIds)
        //{
        //    var wallets = await _context.Wallets
        //        .Where(w => walletIds.Contains(w.Id))
        //        .ToListAsync();

        //    for (int i = 0; i < walletIds.Count; i++)
        //    {
        //        var wallet = wallets.FirstOrDefault(w => w.Id == walletIds[i]);
        //        if (wallet != null)
        //        {
        //            wallet.DisplayOrder = i;
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //    return new JsonResult(new { success = true });
        //}
    }
}
