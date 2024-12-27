using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.ReportModel;

namespace Memoirist_V2.StoryService.RepoPattern.ReportRepo;

public interface IReportRepository {
	Task<ReportStory> reportStory(ReportStory item);
	Task<List<ReportStory>> getListReported();
	Task<List<ReportStory>> acceptReport(int reportId);
	Task<List<ReportStory>> rejectReport(int reportId);
}
