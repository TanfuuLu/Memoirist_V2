using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.StoryService.Models;

public class Chapter {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int ChapterId { get; set; }
	public string? ChapterTitle { get; set; }
	public string? ChapterContext { get; set; }
	public int? ChapterNumber { get; set; }
	public string? ChapterDateTime { get; set; }

	public int StoryId { get; set; }
	public Story? Story { get; set; }
}
