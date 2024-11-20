namespace Memoirist_V2.PostService.Models.DTO;

public class AddCommentPost {
	public string? CommentContext { get; set; }
	public int? CommentWriterId { get; set; }
	public string? CommentWriterAvatar { get; set; }
	public string? CommentWriterName { get; set; }
	public int PostId { get; set; }

}
