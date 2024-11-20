using Memoirist_V2.StoryService.Models;

namespace Memoirist_V2.StoryService.RepoPattern.ChapterRepo;

public interface IChapterRepository {
	Task<Chapter> AddChapter(int storyId, Chapter chapter);
	Task<Chapter> DeleteChapter( int chapter);
	Task<List<Chapter>> GetAllChapterOfStory(int storyId);
	Task<Chapter> GetChapterToRead(int storyId, int chapterId);
}
