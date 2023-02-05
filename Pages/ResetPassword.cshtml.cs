using FreshFarmMarket.Models;
using FreshFarmMarket.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace FreshFarmMarket.Pages
{
    public class ResetPassword : PageModel
    {
        [BindProperty]
        public ResetPasswordModel resetPasswordModel { get; set; }
        private readonly UserManager<AppUser> _userManager;
        public ResetPassword(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet(string code, string email)
        {
            if (code == null || email == null)
            {
                TempData["FlashMessage.Text"] = "Invalid Password Reset Link";
                TempData["FlashMessage.Type"] = "text-danger";
                RedirectToPage("/Dashboard");
            }
            resetPasswordModel = new ResetPasswordModel
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code!)),
                Email = email!
            };
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"resetPasswordModel.Code {resetPasswordModel.Code}");
            Console.WriteLine($"resetPasswordModel.Email {resetPasswordModel.Email}");
            Console.WriteLine($"resetPasswordModel.Password {resetPasswordModel.Password}");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    TempData["FlashMessage.Text"] = "Password reset failed";
                    TempData["FlashMessage.Type"] = "text-danger";
                    return RedirectToPage("/Dashboard");
                }

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Code, resetPasswordModel.Password);
                Console.WriteLine($"result {result}");
                if (result.Succeeded)
                {
                    TempData["FlashMessage.Text"] = "Password reset success";
                    TempData["FlashMessage.Type"] = "text-success";
                    return RedirectToPage("/Dashboard");
                }
            }
            TempData["FlashMessage.Text"] = "Password reset failed";
            TempData["FlashMessage.Type"] = "text-danger";

            return Page();
        }
    }
}
