using Memoirist_V2.PostService.DataContext;
using Memoirist_V2.PostService.Models;
using Memoirist_V2.PostService.RepoPattern.RabbitMQ;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.PostService.RepoPattern;

public class PostRepository : IPostRepository {
	private readonly PostDbContext postDbContext;
	private readonly IPostRabbitRepository postRabbitRepository;

	public PostRepository(PostDbContext postDbContext, IPostRabbitRepository postRabbitRepository) {
		this.postDbContext = postDbContext;
		this.postRabbitRepository = postRabbitRepository;
	}

	public async Task<Post> AddPost(Post postItem) {
		if(postItem != null) {
			postDbContext.Posts.Add(postItem);
			
		}
		await postDbContext.SaveChangesAsync();
		return postItem;
	}

	public async Task<Post> DeletePost(int id) {
		if(id == null) {
			throw new ArgumentNullException("id");
		}
		var item = await postDbContext.Posts.FirstOrDefaultAsync(x => x.PostId == id);
		postDbContext.Posts.Remove(item);
		await postDbContext.SaveChangesAsync();
		return item;
	}

	public async Task<List<Post>> GetAllPost() {
		var listItem = await postDbContext.Posts.ToListAsync();
		return listItem;
	}

	public async Task<Post> GetPostById(int id) {
		var findItem = await postDbContext.Posts.FirstOrDefaultAsync(x => x.PostId == id);
		if(findItem == null ) {
			return null;
		}
		return findItem;
	}

	public async Task<Post> UpdatePostContext(int id, string newContext) {
		var findItem = await postDbContext.Posts.FirstOrDefaultAsync(x => x.PostId == id);
		if(findItem == null) {
			return null;
		}
		if(newContext != "string") {
			findItem.PostContext = newContext;
		}
		await postDbContext.SaveChangesAsync();
		return findItem;
	}
}
