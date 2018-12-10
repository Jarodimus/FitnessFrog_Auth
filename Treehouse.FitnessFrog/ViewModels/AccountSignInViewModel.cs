using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treehouse.FitnessFrog.ViewModels
{
    public class AccountSignInViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Display (Name="Remember me?")]
        public bool RememberMe { get; set; }
    }
}
