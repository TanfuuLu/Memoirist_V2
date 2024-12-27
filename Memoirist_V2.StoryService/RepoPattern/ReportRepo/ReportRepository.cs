using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.ReportModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Memoirist_V2.StoryService.RepoPattern.ReportRepo;

public class ReportRepository : IReportRepository{
	private readonly StoryDbContext storyDbContext;

	public ReportRepository(StoryDbContext storyDbContext) {
		this.storyDbContext = storyDbContext;
	}

	public async Task<ReportStory> reportStory(ReportStory item) {
		storyDbContext.reportStories.Add(item);
		await storyDbContext.SaveChangesAsync();
		return item;

	}

	public async Task<List<ReportStory>> getListReported() {
		var listDomain = await storyDbContext.reportStories.ToListAsync();


		return listDomain;
	}

	public async Task<List<ReportStory>> acceptReport(int reportId) {
		var itemDomain = await storyDbContext.reportStories.FirstOrDefaultAsync(r => r.reportId == reportId);
		if(itemDomain != null) {
			var storyItem = await storyDbContext.Stories.FirstOrDefaultAsync(s => s.StoryId == itemDomain.storyReportId);
			storyDbContext.Stories.Remove(storyItem);
			storyDbContext.reportStories.Remove(itemDomain);
			await storyDbContext.SaveChangesAsync();
			return await storyDbContext.reportStories.ToListAsync();
		} else {
			return await storyDbContext.reportStories.ToListAsync();

		}

	}

	public async Task<List<ReportStory>> rejectReport(int reportId) {
		var itemDomain = await storyDbContext.reportStories.FirstOrDefaultAsync( r=> r.reportId == reportId);
		if(itemDomain != null) {
			storyDbContext.reportStories.Remove(itemDomain);
			await storyDbContext.SaveChangesAsync();
			return await storyDbContext.reportStories.ToListAsync();
		} else {
			return await storyDbContext.reportStories.ToListAsync();
		}
		
	}
}
