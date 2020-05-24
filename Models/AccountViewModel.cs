using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YetAnotherDemo.Models
{
    public class AccountViewModel
    {
        public List<string> Roles { get; set; }
        public Account Account { get; set; }

        public AccountViewModel()
        {
            Roles = new List<String>();
            Account = new Account();
        }
    }

    public class Account
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression("[A-Z][a-zA-Z0-9]+", ErrorMessage = "First letter must be capital and only alpha-numeric characters are allowed")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
