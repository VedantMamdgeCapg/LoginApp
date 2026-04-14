using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
	public class RegisterViewModel
	{
		[Required]
		[StringLength(50)]
		public string FullName { get; set; } = "";

		[Required]
		[EmailAddress]     
		public string Email { get; set; } = "";

		[Required]
		[DataType(DataType.Password)]
		[StringLength(100, MinimumLength = 6)]
		public string Password { get; set; } = "";

		[Required]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; } = "";
	}
}
