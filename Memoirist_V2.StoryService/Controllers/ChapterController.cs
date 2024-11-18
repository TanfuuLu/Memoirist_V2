using AutoMapper;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.DTO;
using Memoirist_V2.StoryService.RepoPattern.ChapterRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memoirist_V2.StoryService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChapterController : ControllerBase {
	private readonly IChapterRepository chapterRepository;
	private readonly IMapper mapper;

	public ChapterController(IChapterRepository chapterRepository, IMapper mapper) {
		this.chapterRepository = chapterRepository;
		this.mapper = mapper;
	}
	[HttpPost("post-{id:int}/add-chapter")]
	public async Task<IActionResult> AddChapter([FromRoute]int id, AddChapterDTO itemDto) {
		var itemDomain = mapper.Map<Chapter>(itemDto);
		var item = await chapterRepository.AddChapter(id, itemDomain);
		return Ok(item);

	}
}
