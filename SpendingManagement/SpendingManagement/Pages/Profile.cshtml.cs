using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserService userService;

        public ProfileModel(UserService userService)
        {
            this.userService = userService;
        }

        public User CurrentUser { get; set; }

        //[BindProperty]
        //public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            CurrentUser = userService.GetUserById(userId.Value);
            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear(); // Clear session
            return RedirectToPage("/Login");
        }

        public IActionResult OnPostChangePassword()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return Page();
            }

            bool result = userService.ChangePassword(userId.Value, NewPassword);

            if (!result)
            {
                ModelState.AddModelError("", "Incorrect current password.");
                return Page();
            }

            return RedirectToPage();
        }
    }
}
