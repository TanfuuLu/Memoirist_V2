using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.PostService.Models;

public class Post {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int PostId { get; set; }
	public int? PostWriterId { get; set; }
	public string? PostWriterAvatar {  get; set; }
	public string? PostWriterName { get; set; }
	public string? PostDateTime { get; set; }
	public string? PostContext {  get; set; }
	public int? PostLike { get; set; }
	public List<CommentPost> ListCommentPost { get; set; } = new List<CommentPost>();
	public List<int>? ListWriterLikePost { get; set; } = new List<int>();
}
