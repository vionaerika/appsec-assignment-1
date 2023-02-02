using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace FreshFarmMarket.Pages;

public class Register : PageModel
{
    [BindProperty]
    public string FullName { get; set; } = string.Empty;
    [BindProperty]
    public string CreditCardNo { get; set; } = string.Empty;
    [BindProperty]
    public string Gender { get; set; } = string.Empty;
    [BindProperty]
    public string MobileNo { get; set; } = string.Empty;
    [BindProperty]
    public string DeliveryAddress { get; set; } = string.Empty;
    [BindProperty]
    public string EmailAddress { get; set; } = string.Empty;
    [BindProperty]
    public string Password { get; set; } = string.Empty;
    [BindProperty]
    public string ConfirmPassword { get; set; } = string.Empty;
    [BindProperty]
    public string Photo { get; set; } = string.Empty;
    [BindProperty]
    public string AboutMe { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        //// Registration Form
        // encrypt email input in the register form
        // pesudo code
        // encrpyted_email = encrypt(email)

        // attempt to fetch existing data from database using encrypted email
        // for sql statement, please use prepared statements, below line is example ONLY
        // prepared statement reference: https://nyplms.polite.edu.sg/d2l/le/enhancedSequenceViewer/168707?url=https%3A%2F%2F5ff0cccf-42fe-41ae-a18f-a4e0f77dec33.sequences.api.brightspace.com%2F168707%2Factivity%2F5487882%3FfilterOnDatesAndDepth%3D1
        // isExistingEmail = f"SELECT user from USER where email = {encrpyted_email}"

        // if email exist in database, prmopt user to enter again

        // else if email does not exist
        // consolidate all form fields data 
        // encrypt credit card number
        // check photo is jpg
        // encode about me values

        //// Securing credentials
        // check that user is using strong password

        //// Session
        // after checking password, and everything is successfully saved to database
        // create a session for the user
        // session management reference: https://nyplms.polite.edu.sg/content/enforced/168707-22S2-IT2163/csfiles/home_dir/courses/2021S2-IT2163/L04%20-%20Session%20Management%20and%20Security.pdf?_&d2lSessionVal=c5U1QEWT0N0vW7aZoy1KTeqA6&ou=168707

        return Page();
    }
}