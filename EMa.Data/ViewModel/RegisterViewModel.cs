using System.ComponentModel.DataAnnotations;

namespace EMa.Data.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string ChildName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
