using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
    public class ChangeUsernameViewModel
    {
        [Required]
        [StringLength(50)]
        public string NewUserName { get; set; } = string.Empty;
    }
}
