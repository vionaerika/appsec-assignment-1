using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using FreshFarmMarket.Models;

namespace FreshFarmMarket.Pages
{
    [Authorize]
    public class Dashboard : PageModel
    {
        [BindProperty]
        public AppUser appUser { get; set; } = new AppUser();
        [BindProperty]
        public string Role { get; set; } = string.Empty;
        private UserManager<AppUser> _userManager;
        public Dashboard(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            Console.WriteLine("ENTERED");
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user); // returns a list

            // get protector for decryption
            var dataProtectionProvider = DataProtectionProvider.Create("FreshFarmMarket");
            var dataProtector = dataProtectionProvider.CreateProtector("FreshFarmMarketKey");

            user.Photo = Encoding.UTF8.GetString(Convert.FromBase64String(user.Photo));
            user.AboutMe = Encoding.UTF8.GetString(Convert.FromBase64String(user.AboutMe));
            user.DeliveryAddress = Encoding.UTF8.GetString(Convert.FromBase64String(user.DeliveryAddress));
            user.CreditCardNo = dataProtector.Unprotect(user.CreditCardNo);

            foreach (var i in roles)
            {
                Role += i;
            }

            appUser = user;

            return Page();
        }
    }
}