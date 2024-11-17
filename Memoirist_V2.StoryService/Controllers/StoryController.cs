using AutoMapper;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.DTO;
using Memoirist_V2.StoryService.RepoPattern.RabbitMess;
using Memoirist_V2.StoryService.RepoPattern.StoryRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Memoirist_V2.StoryService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StoryController : ControllerBase {
	private readonly IRabbitRepository rabbitRepository;
	private readonly IStoryRepository	storyRepository;
	private readonly IMapper mapper;

	public StoryController(IRabbitRepository rabbitRepository, IStoryRepository storyRepository, IMapper mapper) {
		this.rabbitRepository = rabbitRepository;
		this.storyRepository = storyRepository;
		this.mapper = mapper;
	}

	[HttpGet("get-list-story")]
	public async Task<IActionResult> GetListStory() {
		var result = await storyRepository.GetListStory();
		return Ok(result);
	}
	[HttpPost("add-story")]
	public async Task<IActionResult> AddStory(AddStoryDTO itemDTO) {
		////StoryAuthor sẽ bằng tên của Username truyền vào từ Frontend
		////StoryWriterId cung vay
		var itemDomain = mapper.Map<Story>(itemDTO);
		var result = await storyRepository.AddStory(itemDomain);
		return Ok(result);
	}
	[HttpGet("get-story-{storyId:int}")]
	public async Task<IActionResult> GetStory([FromRoute]int storyId) {
		var item = await storyRepository.GetStory(storyId);
		var mapItem = mapper.Map<ShowStoryDTO>(item);
		return Ok(mapItem);
	}
	[HttpGet("get-list-following-of-writer")]
	public async Task<IActionResult> GetFollowingOfWriter() {
		var listItem = await storyRepository.GetListStoryFollowing();
		return Ok(listItem);
	}
	[HttpGet("get-list-story-of-writer")]
	public async Task<IActionResult> GetStoryOfWriter() {
		var listItem = await storyRepository.GetListStoryOfWriter();
		return Ok(listItem);
	}
	[HttpDelete("delete-story/{id:int}")]
	public async Task<IActionResult> DeleteStory([FromRoute]int id) {
		var item = await storyRepository.DeleteStory(id);
		return Ok(item);
	}
}
