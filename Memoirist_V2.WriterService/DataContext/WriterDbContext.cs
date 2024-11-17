using Memoirist_V2.WriterService.Models;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.WriterService.DataContext;

public class WriterDbContext : DbContext {
	public WriterDbContext(DbContextOptions<WriterDbContext> options) : base(options) {
	}
	public DbSet<Writer> Writers { get; set; }
}
