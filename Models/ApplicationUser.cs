using Microsoft.AspNetCore.Identity;

namespace RoushUSPS_App.Models
{
	public class ApplicationUser : IdentityUser	
	{

		public virtual List<AddressEntity> Addresses { get; set; }
	}
}
