using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class WalletsModel : PageModel
    {
        private readonly WalletService _walletService;
        public WalletsModel(WalletService walletService)
        {
            _walletService = walletService;
        }

        [BindProperty]
        public string WalletName { get; set; }

        [BindProperty]
        public decimal WalletBalance { get; set; }
        public List<Wallet> Wallets { get; set; } = new();
        public decimal TotalBalance { get; set; }
        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                Wallets = _walletService.GetAll(userId.Value);
                // Calculate total balance
                TotalBalance = Wallets.Sum(w => w.Balance ?? 0);

                return Page();
            }
            else
            {
                return RedirectToPage("/Login");
            }

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
