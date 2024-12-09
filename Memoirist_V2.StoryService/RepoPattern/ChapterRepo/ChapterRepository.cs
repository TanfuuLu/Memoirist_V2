using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.RepoPattern.RabbitMess;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.StoryService.RepoPattern.ChapterRepo;

public class ChapterRepository : IChapterRepository {
	private readonly StoryDbContext storyDbContext;

	public ChapterRepository(StoryDbContext storyDbContext) {
		this.storyDbContext = storyDbContext;
	}

	public async Task<Chapter> AddChapter(int storyId, Chapter chapter) {
		var listChapterOfStory = await storyDbContext.Chapters.Where(c => c.StoryId == storyId).ToListAsync();
		DateTime datetime = DateTime.Now;
		chapter.ChapterDateTime = datetime.ToString("dd/MM/yyyy");
		var lastChapter = listChapterOfStory.LastOrDefault();
		if(lastChapter != null) {
			chapter.ChapterNumber = lastChapter.ChapterNumber + 1;
		} else {
			chapter.ChapterNumber = 1;
		}
		chapter.StoryId = storyId;
		storyDbContext.Chapters.Add(chapter);
		await storyDbContext.SaveChangesAsync();
		return chapter;
	}

	public async Task<Chapter> DeleteChapter(int chapterId) {
		var findChapter = await storyDbContext.Chapters.FirstOrDefaultAsync(c => c.ChapterId == chapterId);
		if(findChapter != null) {
			storyDbContext.Chapters.Remove(findChapter);
			await storyDbContext.SaveChangesAsync();
			return findChapter; 
		} else {
			return null;
		}
	}

	public async Task<List<Chapter>> GetAllChapterOfStory(int storyId) {
		var listDomain = await storyDbContext.Chapters.ToListAsync();
		List<Chapter> chapterList = new List<Chapter>();
		foreach(var item in listDomain) {
			if(item.StoryId == storyId) {
				chapterList.Add(item);
			}
		}
		return chapterList;
	}

	public async Task<Chapter> GetChapterToRead(int storyId, int chapterId) {
		var listChapter = await storyDbContext.Chapters.ToListAsync();
		List<Chapter> listResult = new List<Chapter>();
		foreach(var item in listChapter) {
			if(item.StoryId == storyId) {
				listResult.Add(item);
			}
		}
		var itemChapter = listResult.FirstOrDefault(x => x.ChapterId == chapterId);
		return itemChapter;
	}
}
