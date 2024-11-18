namespace Memoirist_V2.StoryService.Models.DTO;

public class AddChapterDTO {
	public int StoryId { get; set; }
	public string? ChapterTitle { get; set; }
	public string? ChapterContext { get; set; }
	public int? ChapterNumber { get; set; }
	
}
