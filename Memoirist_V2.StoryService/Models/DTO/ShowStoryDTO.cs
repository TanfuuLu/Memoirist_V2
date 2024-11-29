﻿namespace Memoirist_V2.StoryService.Models.DTO;

public class ShowStoryDTO {
	public string? StoryName { get; set; }
	public string? StoryIntroduction { get; set; }
	public string? StoryAuthor { get; set; }
	public int? StoryLikes { get; set; }
	public string? StoryPicture { get; set; }
	public string? StoryDateWrited { get; set; }
}
