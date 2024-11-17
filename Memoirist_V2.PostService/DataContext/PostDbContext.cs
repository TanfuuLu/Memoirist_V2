using Memoirist_V2.PostService.Models;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.PostService.DataContext;

public class PostDbContext : DbContext {
	public PostDbContext(DbContextOptions<PostDbContext> options) : base(options) {
	}
	public DbSet<Post> Posts { get; set; }
	public DbSet<CommentPost> CommentPosts { get; set; }
}
