using Memoirist_V2.PostService.Models;

namespace Memoirist_V2.PostService.RepoPattern;

public interface IPostRepository {
	Task<Post> AddPost(Post postItem);
	Task<Post> UpdatePostContext(int id,string newContext);
	Task<Post> DeletePost(int id);
	Task<Post> GetPostById(int id);
	Task<List<Post>> GetAllPost();
	Task<List<Post>> GetListByWriterId(int writerId);
}
