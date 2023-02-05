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
    public void OnGet()
    {

    }
}