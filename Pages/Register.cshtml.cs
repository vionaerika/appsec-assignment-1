using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Encodings.Web;

using FreshFarmMarket.Models;
using FreshFarmMarket.Models.DTO;

namespace FreshFarmMarket.Pages;

public class Register : PageModel
{
    [BindProperty]
    public RegisterModel registerModel { get; set; }
    [BindProperty]
    public IFormFile? PhotoUpload { get; set; }

    private UserManager<AppUser> _userManager;
    private IWebHostEnvironment _environment;

    public Register(UserManager<AppUser> userManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _environment = environment;
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);

                if (existingUser != null)
                {
                    TempData["FlashMessage.Text"] = "Email has been used";
                    TempData["FlashMessage.Type"] = "text-danger";
                    return Page();
                }
                var imageRefPath = "";

                if (PhotoUpload != null)
                {
                    if (PhotoUpload.ContentType == "image/jpeg" || PhotoUpload.ContentType == "image/jpg")
                    {
                        var imageFolder = "uploads/images";
                        var imageFile = Guid.NewGuid() + Path.GetExtension(PhotoUpload.FileName);
                        var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", imageFolder, imageFile);
                        using var fileStream = new FileStream(imagePath, FileMode.Create);
                        await PhotoUpload.CopyToAsync(fileStream);
                        imageRefPath = string.Format("/{0}/{1}", imageFolder, imageFile);
                    }
                    else
                    {
                        TempData["FlashMessage.Text"] = "Only JPG Image files are allowed";
                        TempData["FlashMessage.Type"] = "text-danger";
                        return Page();
                    }
                }
                else
                {
                    var imageFolder = "uploads/images";
                    var imageFile = "default_user.jpg";
                    Path.Combine(_environment.ContentRootPath, "wwwroot", imageFolder, imageFile);
                    imageRefPath = string.Format("/{0}/{1}", imageFolder, imageFile);
                }

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var dataProtector = dataProtectionProvider.CreateProtector("MySecretKey");

                var user = new AppUser()
                {
                    FullName = registerModel.FullName,
                    CreditCardNo = dataProtector.Protect(registerModel.CreditCardNo),
                    Gender = registerModel.Gender,
                    MobileNo = registerModel.MobileNo,
                    DeliveryAddress = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerModel.DeliveryAddress)),
                    Email = registerModel.Email,
                    UserName = registerModel.Email,
                    Photo = Convert.ToBase64String(Encoding.UTF8.GetBytes(imageRefPath)),
                    AboutMe = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerModel.AboutMe)),
                    // TwoFactorEnabled = true
                };

                var res = await _userManager.CreateAsync(user, registerModel.Password);
                if (res.Succeeded)
                {
                    return RedirectToPage("/Login");
                }
            }
            TempData["FlashMessage.Type"] = "text-danger";
            TempData["FlashMessage.Text"] = "Account registration failed";
            return Page();
        }
        catch (Exception exc)
        {
            Console.WriteLine($"Exceptions {exc}");
            return RedirectToPage("/Errors/500");
        }
    }
}