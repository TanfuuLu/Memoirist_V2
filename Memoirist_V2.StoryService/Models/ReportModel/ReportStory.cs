using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Memoirist_V2.StoryService.Models.ReportModel;

public class ReportStory {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int reportId {  get; set; }
	public int storyReportId { get; set; }
	public string? Violation { get; set; }
	public string? DateTimeReport {  get; set; }
	public string? storyWriterName { get; set; }
	public string? storyName { get; set; }

}
