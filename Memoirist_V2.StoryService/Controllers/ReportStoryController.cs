using Memoirist_V2.StoryService.Models.ReportModel;
using Memoirist_V2.StoryService.RepoPattern.ReportRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memoirist_V2.StoryService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportStoryController : ControllerBase {
	private readonly IReportRepository reportRepository;

	public ReportStoryController(IReportRepository reportRepository) {
		this.reportRepository = reportRepository;
	}

	[HttpPost("report-story")]
	public async Task<IActionResult> ReportStory(ReportStory item) {
		if(item == null) {
			return BadRequest();
		} else {
			var result = await reportRepository.reportStory(item);
			return Ok(result);
		}
	}
	
	[HttpGet("get-reported-story")]
	public async Task<IActionResult> GetListReportedStory() {
		var result = await reportRepository.getListReported();
		return Ok(result);
	}
	[HttpGet("accept-report/{id:int}")]
	public async Task<IActionResult> AcceptReport([FromRoute] int id) {
		var list = await reportRepository.acceptReport(id);
		return Ok(list);
	}
	[HttpGet("reject-report/{id:int}")]
	public async Task<IActionResult> RejectReport([FromRoute] int id) {
		var list = await reportRepository.rejectReport(id);
		return Ok(list);
	}
}
