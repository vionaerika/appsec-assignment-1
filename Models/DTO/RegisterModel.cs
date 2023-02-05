using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Models.DTO
{
    public class RegisterModel
    {
        [Required, MaxLength(50), DisplayName("Full Name"), RegularExpression("[A-z\\s]+", ErrorMessage = "Please enter name with only alphabets and spaces.")]
        public string FullName { get; set; } = string.Empty;

        [Required, DisplayName("Credit Card Number"), RegularExpression("^[0-9]{16}$", ErrorMessage = "Please enter credit card number in 16 digits format.")]
        public string CreditCardNo { get; set; } = string.Empty;

        [Required, MaxLength(1), RegularExpression("^(M|F)$", ErrorMessage = "Gender must be either M or F")]
        public string Gender { get; set; } = string.Empty;

        [Required, MaxLength(8), DisplayName("Mobile Number"), DataType(DataType.PhoneNumber), RegularExpression("^[0-9]{8}$", ErrorMessage = "Please enter mobile number in 8 digits format.")]
        public string MobileNo { get; set; } = string.Empty;

        [Required, DisplayName("Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required, DataType(DataType.EmailAddress), RegularExpression(@"^[a-z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?(?:\.[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?)*$", ErrorMessage = "Please enter email in valid format")]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), DisplayName("Password"),
            RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{12,}$", ErrorMessage = "Please enter password with minimum 12 characters (at least one upper case, at least one lower case, at least one digit, at least one special character (#?!@$%^&*-)")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), DisplayName("Confirm Password"), Compare("Password", ErrorMessage = "Confirmation password and password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string PhotoURL { get; set; } = string.Empty;

        [Required, DisplayName("About Me")]
        public string AboutMe { get; set; } = string.Empty;
    }
}
