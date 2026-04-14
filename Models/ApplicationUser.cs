using Microsoft.AspNetCore.Identity;

namespace LoginApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}