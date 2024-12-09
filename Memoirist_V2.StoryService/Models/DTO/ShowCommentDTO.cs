
namespace Memoirist_V2.StoryService.Models.DTO;

public class ShowCommentDTO {
	public int CommentId { get; set; }
	public int? CommentLike { get; set; }
	public string? CommentContext { get; set; }
	public string? CommentDateTime { get; set; }
}
