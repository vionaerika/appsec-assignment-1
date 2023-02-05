using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

using FreshFarmMarket.Models;
using FreshFarmMarket.Models.DTO;

namespace FreshFarmMarket.Pages;

public class Login : PageModel
{
    [BindProperty]
    public LoginModel loginModel { get; set; }
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    public Login(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;

    }
    public async Task<IActionResult> OnGet(string userId, string token)
    {
        try
        {
            // userId and token null if user navigate to this page through in-app button
            if (userId == null || token == null)
            {
                return Page();
            }
            // check user validity
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["FlashMessage.Text"] = "User not found";
                TempData["FlashMessage.Type"] = "text-danger";
                return Page();
            }
            // decode and get confirmation token 
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            // check email confirmation
            var res = await _userManager.ConfirmEmailAsync(user, token);
            if (res.Succeeded)
            {
                TempData["FlashMessage.Text"] = "Thank you, email confirmation successful";
                TempData["FlashMessage.Type"] = "text-success";
            }
            else
            {
                TempData["FlashMessage.Text"] = "Email Confirmation failed";
                TempData["FlashMessage.Type"] = "text-danger";
            }
            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Errors/500");
        }
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, lockoutOnFailure: true);
                if (res.IsLockedOut)
                {
                    TempData["FlashMessage.Text"] = "Account locked due to multiple failed login attempts";
                    TempData["FlashMessage.Type"] = "text-danger";
                    return Page();
                }
                if (res.Succeeded)
                {
                    return RedirectToPage("/Dashboard");
                }
                else
                {
                    TempData["FlashMessage.Text"] = "Invalid credentials";
                    TempData["FlashMessage.Type"] = "text-danger";
                    return Page();
                }
            }
            TempData["FlashMessage.Text"] = "Invalid login attempt";
            TempData["FlashMessage.Type"] = "text-danger";
            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Errors/500");
        }
    }
}