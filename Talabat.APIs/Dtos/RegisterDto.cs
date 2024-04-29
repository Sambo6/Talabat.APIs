using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class RegisterDto
	{
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$", ErrorMessage = "Password must be Minimum 8 characters & at least 1 uppercase letter & 1 lowercase letter & 1 number")]
        public string Password { get; set; } = null!;
		[Required]
		public string Phone { get; set; } = null!;

	}
}
