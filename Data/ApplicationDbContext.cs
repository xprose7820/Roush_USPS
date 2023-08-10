using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoushUSPS_App.Models;

namespace RoushUSPS_App.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<AddressEntity> Addresses { get; set; }
		public DbSet<ApplicationUser> Users { get; set; }

	}
}
