using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;
using System.Security.Cryptography.Xml;

namespace SpendingManagement.Pages
{
    public class WalletDetailModel : PageModel
    {
        private readonly WalletService _walletService;
        public WalletDetailModel(WalletService walletService)
        {
            _walletService = walletService;
        }
        [BindProperty]
        public Wallet Wallet { get; set; }
        public IActionResult OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                Wallet = _walletService.GetWalletById(id, userId.Value);

                if (Wallet == null)
                {
                    return NotFound();
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Login");
            }

        }
        public IActionResult OnPostUpdate()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _walletService.UpdateWallet(Wallet, userId.Value);
            return RedirectToPage("/Wallets");
        }

        public IActionResult OnPostDelete()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            _walletService.DeleteWallet(Wallet.Id, userId.Value);
            return RedirectToPage("/Wallets");
        }

        //public IActionResult OnPostTransfer()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Wallets = _walletService.GetAllWallets();
        //        return Page();
        //    }

        //    _transactionService.TransferMoney(Transfer);
        //    return RedirectToPage("/Wallets");
        //}
    }
}
