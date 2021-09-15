using System.ComponentModel.DataAnnotations;

namespace EMa.Data.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
