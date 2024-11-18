namespace Memoirist_V2.StoryService.Models.DTO;

public class AddStoryDTO {
	public int? WriterStoryId { get; set; }
	public string? StoryName { get; set; }
	public string? StoryIntroduction { get; set; }
	public string? StoryAuthor { get; set; }
	public string? StoryPicture { get; set; }
	public string? StoryDateWrited { get; set; }
	public bool TermsAndConditionsCheck { get; set; }

}
