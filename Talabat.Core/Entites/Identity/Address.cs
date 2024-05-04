using System.Text.Json.Serialization;

namespace Talabat.Core.Entities.Identity
{
	public class Address : BaseEntity
	{
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Street { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Country { get; set; } = null!;
		//[JsonIgnore] // for nested looping between Address & User
		public string ApplicationUserId { get; set; } //Foreign key
		//[JsonIgnore]
		public ApplicationUser User { get; set; } = null!; //Navigational property [one]
	}
}