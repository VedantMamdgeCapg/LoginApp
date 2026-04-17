using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
