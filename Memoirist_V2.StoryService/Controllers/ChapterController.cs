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
	[HttpPost("story-{id:int}/add-chapter")]
	public async Task<IActionResult> AddChapter([FromRoute] int id, [FromBody] AddChapterDTO itemDto) {
		var itemDomain = mapper.Map<Chapter>(itemDto);
		var item = await chapterRepository.AddChapter(id, itemDomain);
		return Ok(item);
	}
	[HttpGet("story-{id:int}/get-all-chapter")]
	public async Task<IActionResult> GetAllChapterOfStory([FromRoute] int id) {
		var listItem = await chapterRepository.GetAllChapterOfStory(id);
		return Ok(listItem);
	}
	[HttpGet("story-{idStory:int}/chapter-{idChapter:int}")]
	public async Task<IActionResult> GetChapterToRead([FromRoute] int idStory, [FromRoute] int idChapter) {
		var item = await chapterRepository.GetChapterToRead(idStory, idChapter);
		var itemMap = mapper.Map<ShowChapterDTO>(item);
		return Ok(itemMap);
	}
	[HttpDelete("delete-chapter/{idChapter:int}")]
	public async Task<IActionResult> DeleteChapter([FromRoute] int idChapter) {
		var item = await chapterRepository.DeleteChapter(idChapter);
		if(item != null) {
			 
			return Ok();
		} else {
			return BadRequest();
		}
	}
}
