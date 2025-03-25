using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class AddTransactionModel : PageModel
    {
        private readonly TransactionService _transactionService;
        private readonly WalletService _walletService;

        [BindProperty]
        public Transaction Transaction { get; set; } = new Transaction();

        public List<Wallet> Wallets { get; set; }

        [BindProperty(SupportsGet = true)]
        public int WalletId { get; set; }

        public AddTransactionModel(TransactionService transactionService, WalletService walletService)
        {
            _transactionService = transactionService;
            _walletService = walletService;
        }

        public void OnGet()
        {
            //Wallets = _walletService.GetAll();

            //// Set WalletId if passed via URL
            //if (WalletId != 0)
            //{
            //    //Transaction.WalletId = WalletId;
            //}
        }

        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Wallets = _walletService.GetAllWallets();
        //        return Page();
        //    }

        //    Transaction.Date = DateTime.Now;
        //    _transactionService.AddTransaction(Transaction);

        //    return RedirectToPage("/Transactions", new { WalletId = Transaction.WalletId });
        //}
    }
}
