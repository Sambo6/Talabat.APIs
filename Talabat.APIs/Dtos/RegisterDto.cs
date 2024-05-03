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
        
        public string Password { get; set; } = null!;
		[Required]
		public string Phone { get; set; } = null!;

	}
}
