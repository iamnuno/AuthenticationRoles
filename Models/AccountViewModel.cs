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
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
