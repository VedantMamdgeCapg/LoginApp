using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";

        public string Token { get; set; } = "";
    }
}
