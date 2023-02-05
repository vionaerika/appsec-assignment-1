using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
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
    private RoleManager<IdentityRole> _roleManager;
    private EmailSender _emailSender;
    private IWebHostEnvironment _environment;

    public Register(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, EmailSender emailSender, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailSender = emailSender;
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

                // process uploaded image
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
                        imageRefPath = $"/{imageFolder}/{imageFile}";
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
                    imageRefPath = $"/{imageFolder}/{imageFile}";
                }

                // create protector for encryption
                var dataProtectionProvider = DataProtectionProvider.Create("FreshFarmMarket"); // string value is the unique ID for this application to distinguish from other applications on the machine
                var dataProtector = dataProtectionProvider.CreateProtector("FreshFarmMarketKey"); // string value is the purpose of this newly created protector

                // create new user
                var user = new AppUser()
                {
                    FullName = registerModel.FullName,
                    // encrypt credit card no
                    CreditCardNo = dataProtector.Protect(registerModel.CreditCardNo),
                    Gender = registerModel.Gender,
                    MobileNo = registerModel.MobileNo,
                    Email = registerModel.Email,
                    UserName = registerModel.Email,
                    // encode values to prevent attacks
                    DeliveryAddress = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerModel.DeliveryAddress)),
                    Photo = Convert.ToBase64String(Encoding.UTF8.GetBytes(imageRefPath)),
                    AboutMe = Convert.ToBase64String(Encoding.UTF8.GetBytes(registerModel.AboutMe)),
                };

                var res = await _userManager.CreateAsync(user, registerModel.Password);
                if (res.Succeeded)
                {
                    // check if role exists in db
                    if (!await _roleManager.RoleExistsAsync(registerModel.Role))
                    {
                        // create new role in db and attach user to role
                        await _roleManager.CreateAsync(new IdentityRole(registerModel.Role));
                        await _userManager.AddToRoleAsync(user, registerModel.Role);
                    }
                    else
                    {
                        // attach user to role
                        await _userManager.AddToRoleAsync(user, registerModel.Role);
                    }

                    // send account confirmation email to user
                    // generate and encode token
                    var EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    EmailConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(EmailConfirmationToken));

                    var callbackUrl = Url.Page("/Login", pageHandler: null, values: new { userId = user.Id, token = EmailConfirmationToken }, protocol: Request.Scheme);
                    callbackUrl = HtmlEncoder.Default.Encode(callbackUrl!);
                    Console.WriteLine($"Callback {callbackUrl}");

                    var sendEmailSuccess = await _emailSender.SendEmail(user.Email, $"Please confirm your account creation by <a href='{callbackUrl}'>clicking here</a>.");

                    if (sendEmailSuccess)
                    {
                        TempData["FlashMessage.Type"] = "text-success";
                        TempData["FlashMessage.Text"] = $"Account {registerModel.Email} is added. Please check your email";
                        return RedirectToPage("/Login");
                    }
                    TempData["FlashMessage.Type"] = "text-success";
                    TempData["FlashMessage.Text"] = $"Account {registerModel.Email} is added but email failed to be sent. Please try logging in";
                    return RedirectToPage("/Login");
                }
            }
            // return to page and display error message
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