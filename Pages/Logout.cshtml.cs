using FreshFarmMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class Logout : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        public Logout(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnPost(string? redirectUrl)
        {
            await _signInManager.SignOutAsync();
            if (redirectUrl != null)
            {
                // LocalRedirect ensure user is only redirected to this application's route, preventing malicious redirection from happening
                return LocalRedirect(redirectUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}
