﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreshFarmMarket.Models.DTO
{
    public class LoginModel
    {

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
