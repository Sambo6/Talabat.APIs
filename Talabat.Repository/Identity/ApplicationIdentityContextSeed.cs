using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure.Identity
{
	public static class ApplicationIdentityContextSeed
	{
		public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "MOhammed Abdullah",
					Email = "mohamed.6bdalla@outlook.com",
					UserName = "SAMBO",
					PhoneNumber = "01010817090"
				};
				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}
	}
}
