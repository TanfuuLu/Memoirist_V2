using Memoirist_V2.YarpGateway.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.YarpGateway.DataContext;

public class AuthenticateDbContext : IdentityDbContext<User> {
	public AuthenticateDbContext(DbContextOptions<AuthenticateDbContext> options) : base(options) {
	}
	public override DbSet<User> Users { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		var adminId = "1";
		var userId = "2";
		var roles = new List<IdentityRole> {
			new IdentityRole {
				Id = adminId,
				ConcurrencyStamp = adminId,
				Name = "Admin",
				NormalizedName = "admin".ToUpper()
			},
			new IdentityRole {
				Id = userId,
				ConcurrencyStamp = userId,
				Name = "User",
				NormalizedName = "user".ToUpper()
			}
		
		};
		modelBuilder.Entity<IdentityRole>().HasData(roles);
	}
}
