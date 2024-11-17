using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.StoryService.Models;

public class Comment {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int CommentId { get; set; }

	public int? WriterCommentId { get; set; }
	public string? WriterCommentUsername { get; set; }
	public string? CommentContext { get; set; }
	public string? CommentDateTime { get; set; }

	public int StoryId { get; set; }
	public Story? story { get; set; }
}
