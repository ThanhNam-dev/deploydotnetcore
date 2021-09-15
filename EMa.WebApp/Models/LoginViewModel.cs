using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMa.WebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
