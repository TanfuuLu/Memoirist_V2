using Memoirist_V2.StoryService.Models;

namespace Memoirist_V2.StoryService.RepoPattern.StoryRepo;

public interface IStoryRepository{
	Task<List<Story>> GetListStoryOfWriter(int writerId);
	Task<Story> GetStory(int storyId); 
	Task<List<Story>> SearchStory(string? storyName);
	Task<List<Story>> GetListStory();
	Task<List<Story>> GetListStoryFollowing(List<int> followingStoryId);
	Task<Story> AddStory(Story story);
	Task<Story> UpdateStoryIntroduction(string newIntroduction, int id);
	Task<Story> DeleteStory(int storyId);

	Task<List<Comment>> GetListComment(int storyId);
	Task<Comment> AddComment(Comment comment, int writerId, int storyId);
	Task<Comment> DeleteComment(int commentId);

 }
