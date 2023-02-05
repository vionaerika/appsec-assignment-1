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
        public AppUser appUser { get; set; }
        [BindProperty]
        public string Role { get; set; } = string.Empty;
        private UserManager<AppUser> _userManager;
        private EmailSender _emailSender;
        public Dashboard(UserManager<AppUser> userManager, EmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user); // returns a list

            // get protector for decryption
            var dataProtectionProvider = DataProtectionProvider.Create("FreshFarmMarket");
            var dataProtector = dataProtectionProvider.CreateProtector("FreshFarmMarketKey");

            user.Photo = Encoding.UTF8.GetString(Convert.FromBase64String(user.Photo));
            user.AboutMe = Encoding.UTF8.GetString(Convert.FromBase64String(user.AboutMe));
            user.DeliveryAddress = Encoding.UTF8.GetString(Convert.FromBase64String(user.DeliveryAddress));
            user.CreditCardNo = dataProtector.Unprotect(user.CreditCardNo);

            foreach (var role in roles)
            {
                Role = role;
            }

            appUser = user;
        }
        public async Task<IActionResult> OnPost()
        {
            // send password reset email to user
            // generate and encode token
            var PasswordResetToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            PasswordResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(PasswordResetToken));

            var callbackUrl = Url.Page("/ResetPassword", pageHandler: null, values: new { PasswordResetToken, email = appUser.Email }, protocol: Request.Scheme);
            callbackUrl = HtmlEncoder.Default.Encode(callbackUrl!);

            var sendEmailSuccess = await _emailSender.SendEmail(appUser.Email, "Reset Password", $"Reset your password <a href='{callbackUrl}'>clicking here</a>.");
            Console.WriteLine($"sendEmailSuccess: {sendEmailSuccess}");

            if (sendEmailSuccess)
            {
                TempData["FlashMessage.Type"] = "text-success";
                TempData["FlashMessage.Text"] = "Reset Password Email Sent. Please check your email";
            }
            else
            {
                TempData["FlashMessage.Type"] = "text-danger";
                TempData["FlashMessage.Text"] = "Email failed to be sent. Please Try Again";
            }
            return RedirectToPage("/Dashboard");
        }
    }
}