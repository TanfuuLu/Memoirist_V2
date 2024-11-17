using Memoirist_V2.StoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.StoryService.DataContext;

public class StoryDbContext : DbContext {
	public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options) {
	}
	public DbSet<Story> Stories { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Chapter > Chapters { get; set; }
}
