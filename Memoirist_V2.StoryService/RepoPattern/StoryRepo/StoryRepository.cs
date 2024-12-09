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
		//DateTime datetime =DateTime.Now;
		//story.StoryDateWrited = datetime.ToString("dd/dd/yyyy");
		story.StoryLikes = 0;
		
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

	public async Task<List<Story>> GetListStoryOfWriter(int writerId) {
		var storiesDomain = await dbContext.Stories.ToListAsync();
		var listStoryResult = new List<Story>();
		foreach(var item in storiesDomain) {
			if(item.WriterStoryId == writerId) {
				listStoryResult.Add(item);
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
		var stringSearch = storyName.ToLower();
		var item = await dbContext.Stories.FirstOrDefaultAsync(x => x.StoryName.ToLower() == storyName);
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
	public async Task<Comment> AddComment(Comment comment,int writerId, int storyId) {
		DateTime dateTime = DateTime.Now;
		comment.CommentDateTime = dateTime.ToString("dd/MM/yyyy");
		comment.CommentWriterId = writerId;
		comment.CommentLike = 0;
		comment.CommentWriterId = writerId;
		comment.StoryId = storyId;
		if(comment.CommentContext != null) {
			dbContext.Comments.Add(comment);
			await dbContext.SaveChangesAsync();
			return comment;
		} else {
			return null;
		}
	}

	public async Task<Comment> DeleteComment(int commentId) {
		var item = await dbContext.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
		if(item != null) {
			dbContext.Comments.Remove(item);
			await dbContext.SaveChangesAsync();
			return item;
		} else {
			return null;
		}
	}
	public async Task<List<Comment>> GetListComment(int storyId) {
		var listItem = await dbContext.Comments.Where(x => x.StoryId == storyId).ToListAsync();
		return listItem;
	}

}
