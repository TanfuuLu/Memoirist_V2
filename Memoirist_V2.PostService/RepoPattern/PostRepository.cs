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
	#region//PostService
	public async Task<Post> AddPost(Post postItem) {
		DateTime dateTime = DateTime.Now;
		postItem.PostDateTime = dateTime.ToString("dd/MM/yyyy");
		postItem.PostLike = 0;
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
		if(findItem == null) {
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
	public async Task<List<Post>> GetListByWriterId(int writerId) {
		var listDomain = await postDbContext.Posts.ToListAsync();
		List<Post> posts = new List<Post>();
		foreach(var item in listDomain) {
			if(item.PostWriterId == writerId) {
				posts.Add(item);
			}
		}
		return posts;
	}
	#endregion
	public async Task<CommentPost> AddComment(CommentPost commentPost) {
		postDbContext.CommentPosts.Add(commentPost);
		await postDbContext.SaveChangesAsync();
		return commentPost;
	}

	public async Task<CommentPost> UpdateComment(int commentId, string newContext) {
		var findItem = await postDbContext.CommentPosts.FirstOrDefaultAsync(x => x.CommentPostId == commentId);
		if(newContext != "string" && newContext != " ") {
			findItem.CommentContext = newContext;	
		}
		await postDbContext.SaveChangesAsync();
		return findItem;
	}

	public async Task<CommentPost> DeleteCommentPost(int id) {
		var findItem = await postDbContext.CommentPosts.FirstOrDefaultAsync(x => x.CommentPostId == id);
		postDbContext.CommentPosts.Remove(findItem);
		await		postDbContext.SaveChangesAsync();
		return findItem;
	}

	public async Task<List<CommentPost>> GetAllCommentsPost(int idPost) {
		var listDomain = await postDbContext.CommentPosts.ToListAsync();
		List<CommentPost> commenofPost = new List<CommentPost>();
		foreach(var post in listDomain) {
			if(post.PostId == idPost) {
				commenofPost.Add(post);
			}
		}
		return commenofPost;
	}
	public async Task<Post> LikePost(int postId, int writerId) {
		var findPost = await postDbContext.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
		findPost.ListWriterLikePost.Add(writerId);
		await postDbContext.SaveChangesAsync();
		return findPost;
	}
}
