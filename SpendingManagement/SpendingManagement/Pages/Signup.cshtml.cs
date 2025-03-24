using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Models;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class SignupModel : PageModel
    {
        private readonly UserService _userService;

        public SignupModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public IActionResult OnPost()
        {
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            var newUser = new User
            {
                Fullname = FullName,
                Username = FullName,
                Email = Email,
                Password = Password
            };

            bool result = _userService.RegisterUser(newUser);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return Page();
            }

            return RedirectToPage("/Login");
        }
    }
}
