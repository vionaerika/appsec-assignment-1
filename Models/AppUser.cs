using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(70)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string CreditCardNo { get; set; } = string.Empty;
        [Required, MaxLength(1)]
        public string Gender { get; set; } = string.Empty;
        [Required, MaxLength(8)]
        public string MobileNo { get; set; } = string.Empty;
        [Required]
        public string DeliveryAddress { get; set; } = string.Empty;
        [Required]
        public string Photo { get; set; } = string.Empty;
        [Required]
        public string AboutMe { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;
    }
}
