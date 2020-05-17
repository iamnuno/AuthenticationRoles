using System.ComponentModel.DataAnnotations;

namespace YetAnotherDemo.Models
{
    public class AdminViewModel
    {
        public Role Role { get; set; }
        public AccountStatus AccountStatus { get; set; }

        public AdminViewModel()
        {
            Role = new Role();
            AccountStatus = new AccountStatus();
        }
    }

    public class Role
    {
        [Required]
        public string RoleName { get; set; }
    }

    public class AccountStatus
    {
        [Required]
        public string AccountName { get; set; }
    }
}
