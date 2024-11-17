using Memoirist_V2.StoryService.Models;

namespace Memoirist_V2.StoryService.RepoPattern.StoryRepo;

public interface IStoryRepository{
	Task<List<Story>> GetListStoryOfWriter();
	Task<Story> GetStory(int storyId); 
	Task<Story> SearchStory(string? storyName);
	Task<List<Story>> GetListStory();
	Task<List<Story>> GetListStoryFollowing();
	Task<Story> AddStory(Story story);
	Task<Story> UpdateStoryIntroduction(string newIntroduction, int id);
	Task<Story> DeleteStory(int storyId);
}
