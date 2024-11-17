using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.PostService.Models;

public class CommentPost {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int CommentPostId {  get; set; }
	public string? CommentContext { get; set; }
	public int? CommentWriterId {  get; set; }
	public string? CommentWriterAvatar {  get; set; }
	public string? CommentWriterName { get;set; }
	public int? CommentLike {  get; set; }

}
