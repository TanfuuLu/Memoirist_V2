namespace Memoirist_V2.WriterService.Models.DTO;

public class ReadWriterDTO {
	public string? WriterFullname { get; set; }
	public string? WriterUsername { get; set; }
	public string? WriterBio { get; set; }
	public string? WriterGender { get; set; }
	public string? WriterBirthday { get; set; }
	public string? WriterPhone { get; set; }
	//Post
	public List<int>? ListPostId { get; set; }
	public List<int>? ListFollowingStoryId { get; set; }
	public List<int>? ListStoryId { get; set; }
	//User
	public List<int>? ListFollower { get; set; }
	public List<int>? ListFollowing { get; set; }
}
