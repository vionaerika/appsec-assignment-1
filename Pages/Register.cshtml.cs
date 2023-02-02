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
                    TempData["FlashMessage.Text"] = "Email Exist.";
                    TempData["FlashMessage.Type"] = "danger";
                    return Page();
                }

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var imgUrl = "";

                if (PhotoUpload != null)
                {
                    if (PhotoUpload.ContentType == "image/jpeg" || PhotoUpload.ContentType == "image/jpg")
                    {
                        var uploadsFolder = "uploads/images";
                        var imageFile = Guid.NewGuid() + Path.GetExtension(PhotoUpload.FileName);
                        var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                        using var fileStream = new FileStream(imagePath, FileMode.Create);
                        await PhotoUpload.CopyToAsync(fileStream);
                        imgUrl = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                    }
                    else
                    {
                        ModelState.AddModelError("Upload", "Only JPG File Types.");
                        return Page();
                    }
                }
                else
                {
                    var uploadsFolder = "uploads/images";
                    var imageFile = "default_pic.jpg";
                    Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    imgUrl = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }

                //byte[] salt = new byte[16];
                //RandomNumberGenerator.Create().GetBytes(salt);


                var user = new AppUser()
                {
                    FullName = registerModel.FullName,
                    CreditCardNo = protector.Protect(registerModel.CreditCardNo),
                    Gender = registerModel.Gender,
                    MobileNo = registerModel.MobileNo,
                    DeliveryAddress = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerModel.DeliveryAddress)),
                    Email = registerModel.Email,
                    UserName = registerModel.Email,
                    Photo = Convert.ToBase64String(Encoding.UTF8.GetBytes(imgUrl)),
                    AboutMe = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerModel.AboutMe)),
                    TwoFactorEnabled = true
                };

                var res = await _userManager.CreateAsync(user, registerModel.Password);
                if (res.Succeeded)
                {
                    Console.WriteLine($"RES: {res}");
                    return RedirectToPage("/Login");
                }
                TempData["FlashMessage.Type"] = "danger";
                TempData["FlashMessage.Text"] = "Account Creation Failed";
                return Page();
            }
            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("/Errors/500");
        }
    }
}