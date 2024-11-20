using AutoMapper;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.DTO;
using Memoirist_V2.StoryService.RepoPattern.StoryRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memoirist_V2.StoryService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CommentStoryController : ControllerBase {
	private readonly IStoryRepository storyRepository;
	private readonly IMapper mapper;

	public CommentStoryController(IStoryRepository storyRepository, IMapper mapper) {
		this.storyRepository = storyRepository;
		this.mapper = mapper;
	}
	[HttpGet("story-{storyId:int}/list-comment")]
	public async Task<IActionResult> GetListComment([FromRoute]int storyId) {
		var item = await storyRepository.GetListComment(storyId);
		return Ok(item);
	}
	[HttpPost("story-{id:int}/writer-{writerId:int}-add-comment")]
	public async Task<IActionResult> AddCommentToStory([FromRoute]int id, [FromRoute]int writerId, AddCommentDTO item) {
		var mapItem = mapper.Map<Comment>(item);
		var itemResult = await storyRepository.AddComment(mapItem,writerId,id);
		return Ok(itemResult);
	}
	[HttpDelete("story-{id:int}/delete-comment-{idCmt:int}")]
	public async Task<IActionResult> DeleteComment([FromRoute]int id, [FromRoute]int idCmt) {
		var result = await storyRepository.DeleteComment(idCmt);
		return Ok(result);
	}

}
