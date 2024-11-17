using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.StoryService.Models;

public class Story {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int? StoryId { get; set; }
	public int? WriterStoryId { get; set; }
	public string? StoryName { get; set; }
	public string? StoryIntroduction { get; set; }
	public string? StoryAuthor { get; set; }
	public int? StoryLikes { get; set; }
	public string? StoryPicture { get; set; }
	public string? StoryDateWrited { get; set; }
	public bool TermsAndConditionsCheck { get; set; }
	public List<Comment>? StoryComment { get; set; }= new List<Comment>();
	public List<Chapter>? Chapters { get; set; } = new List<Chapter>();
}
