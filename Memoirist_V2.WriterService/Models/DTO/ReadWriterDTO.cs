namespace Memoirist_V2.WriterService.Models.DTO;

public class ReadWriterDTO {
	public string? WriterFullname { get; set; }
	public string? WriterUsername { get; set; }
	public string? WriterBio { get; set; }
	public string? WriterGender { get; set; }
	public string? WriterBirthday { get; set; }
	public string? WriterPhone { get; set; }
	//Post
	public List<int>? ListPostId { get; set; } = new List<int>();
	public List<int>? ListLikesPost { get; set; } = new List<int>();
	public List<int>? ListPostCommented { get; set; } = new List<int>();

	//Story
	public List<int>? ListFollowingStoryId { get; set; } = new List<int>();
	public List<int>? ListStoryId { get; set; } = new List<int>();
	public List<int>? ListStoryCommented { get; set; } = new List<int>();
	//User
	public List<int>? ListFollower { get; set; } = new List<int>();
	public List<int>? ListFollowing { get; set; } = new List<int>();
}
