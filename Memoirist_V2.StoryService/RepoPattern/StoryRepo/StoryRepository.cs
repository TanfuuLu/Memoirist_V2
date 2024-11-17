using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.RepoPattern.RabbitMess;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.StoryService.RepoPattern.StoryRepo;


public class StoryRepository : IStoryRepository {
	private readonly IRabbitRepository rabbitRepository;
	private readonly StoryDbContext dbContext;

	public StoryRepository(IRabbitRepository rabbitRepository, StoryDbContext dbContext) {
		this.rabbitRepository = rabbitRepository;
		this.dbContext = dbContext;
	}
	public async Task<Story> AddStory(Story story) {
		dbContext.Stories.Add(story);
		await dbContext.SaveChangesAsync();
		var item = await dbContext.Stories.FirstOrDefaultAsync(x => x.StoryName == story.StoryName);
		return item;
	}

	public async Task<Story> DeleteStory(int storyId) {
		var item = await dbContext.Stories.FirstOrDefaultAsync(x => x.StoryId == storyId);
		if(item != null) {
			dbContext.Stories.Remove(item);
			await dbContext.SaveChangesAsync();
			return item;
		} else {
			return null;
		}
	}

	public async Task<List<Story>> GetListStory() {
		var listResult = await dbContext.Stories.ToListAsync();
		return listResult;
	}

	public async Task<List<Story>> GetListStoryFollowing() {
		//Nhận danh sách Id của Story được Writer follow
		var listItem = await rabbitRepository.ReceiveListFollowingStoryOfWriter("FollowingStoryIdQueue");
		var listDomain = await dbContext.Stories.ToListAsync();
		var listResult = new List<Story>();
		//Tim kiem cac story trong Database
		foreach(var item in listDomain) {
			foreach(var storyId in listItem) {
				if(storyId == item.StoryId) {
					listResult.Add(item);
				}
			}
		}
		//tra ve database
		return listResult;
	}

	public async Task<List<Story>> GetListStoryOfWriter() {
		var listItem = await rabbitRepository.ReceiveListFollowingStoryOfWriter("StoryOfWriterQueue");
		var storiesDomain = await dbContext.Stories.ToListAsync();
		var listStoryResult = new List<Story>();
		foreach(var item in storiesDomain) {
			foreach(var story in listItem) {
				if(item.StoryId == story) {
					listStoryResult.Add(item);
				}
			}

		}
		return listStoryResult;
	}

	public async Task<Story> GetStory(int storyId) {
		var item = await dbContext.Stories.FirstOrDefaultAsync(x => x.StoryId == storyId);
		if(item != null) {
			return item;
		} else {
			return null;
		}
	}

	public async Task<Story> SearchStory(string? storyName) {
		var item = await dbContext.Stories.FirstOrDefaultAsync(x => x.StoryName == storyName);
		if(item != null) {
			return item;
		} else {
			return null;
		}
	}

	public async Task<Story> UpdateStoryIntroduction(string newIntro, int id) {
		var item = await dbContext.Stories.FirstOrDefaultAsync(x => x.StoryId == id);
		if(item != null) {
			item.StoryIntroduction = newIntro;
			await dbContext.SaveChangesAsync();
			return item;
		} else {
			return null;
		}
	}
}
