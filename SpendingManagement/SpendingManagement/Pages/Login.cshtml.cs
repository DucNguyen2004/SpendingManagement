using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpendingManagement.Services;

namespace SpendingManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserService userService;

        public LoginModel(UserService authService)
        {
            userService = authService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            var user = userService.ValidateUser(Email, Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Username);
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Invalid email or password.";
            return Page();
        }
    }
}
