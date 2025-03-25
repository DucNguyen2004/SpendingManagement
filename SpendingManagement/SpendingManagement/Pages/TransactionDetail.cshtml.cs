using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class TransactionDetailModel : PageModel
    {
        private readonly TransactionService _transactionService;
        private readonly WalletService _walletService;
        public Transaction Transaction { get; set; }
        public List<Wallet> Wallets { get; set; }
        public TransactionDetailModel(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public void OnGet()
        {

        }

        //public IActionResult OnGet(int id)
        //{
        //Transaction = _transactionService.GetTransactionById(id);
        //if (Transaction == null)
        //{
        //    return NotFound();
        //}
        //return Page();
        //}

        //public IActionResult OnPostDelete(int id)
        //{
        //    _transactionService.DeleteTransaction(id);
        //    return RedirectToPage("/Transactions");
        //}
        //public IActionResult OnPostEdit()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Wallets = _walletService.GetAllWallets();
        //        return Page();
        //    }

        //    _transactionService.UpdateTransaction(Transaction);
        //    return RedirectToPage("/Transactions");
        //}
    }

}
