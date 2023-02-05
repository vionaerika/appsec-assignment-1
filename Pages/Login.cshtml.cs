using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
        catch (Exception exc)
        {
            Console.WriteLine($"Exceptions {exc}");
            return RedirectToPage("/Errors/500");
        }
    }
}