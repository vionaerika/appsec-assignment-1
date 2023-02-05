using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Models.DTO
{
    public class ResetPasswordModel
    {
        [Required, DataType(DataType.Password), DisplayName("Password"),
            RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{12,}$", ErrorMessage = "Please enter password with minimum 12 characters (at least one upper case, at least one lower case, at least one digit, at least one special character (#?!@$%^&*-)")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), DisplayName("Confirm Password"), Compare("Password", ErrorMessage = "Confirmation password and password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
