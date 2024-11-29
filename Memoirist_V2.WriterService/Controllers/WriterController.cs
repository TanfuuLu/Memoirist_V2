using AutoMapper;
using Memoirist_V2.WriterService.Models;
using Memoirist_V2.WriterService.Models.DTO;
using Memoirist_V2.WriterService.RepoPattern;
using Memoirist_V2.WriterService.RepoPattern.RabbitMess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Internal;

namespace Memoirist_V2.WriterService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WriterController : ControllerBase {
	private readonly IRabbitWriterRepository rabbitRepository;
	private readonly IWriterRepository writerRepository;
	private readonly IMapper mapper;

	public WriterController(IRabbitWriterRepository rabbitRepository, IWriterRepository writerRepository, IMapper mapper) {
		this.rabbitRepository = rabbitRepository;
		this.writerRepository = writerRepository;
		this.mapper = mapper;
	}
	//Get Profile Writer
	[HttpGet("profile/{id:int}")]
	public async Task<IActionResult> GetWriterById([FromRoute] int id) {
		var writer = await writerRepository.GetWriterById(id);
		if(writer == null) {
			return NotFound("User not found");
		}
		rabbitRepository.SendListInt(writer.ListFollowingStoryId, "FollowingStoryIdQueue");
		rabbitRepository.SendListInt(writer.ListStoryId, "StoryOfWriterQueue");
		rabbitRepository.SendListInt(writer.ListPostId, "PostOfWriterQueue");
		return Ok(writer);

	}
	//Get user when login from authenticate
	[HttpGet("writer-login")]
	public async Task<IActionResult> WriterLogin() {
		var writeritem = await writerRepository.GetWriterLogin();
		var writerLogin = await writerRepository.GetWriterById(writeritem.WriterId);
		rabbitRepository.SendListInt(writerLogin.ListFollowingStoryId, "FollowingStoryIdQueue");
		rabbitRepository.SendListInt(writerLogin.ListStoryId, "WriterStoryIdQueue");
		return Ok(writerLogin);
	}
	//Update information of writer
	[HttpPut("writer-update")]
	public async Task<IActionResult> WriterUpdate(int id, UpdateWriterDTO itemDTO) {
		var itemDomain = mapper.Map<Writer>(itemDTO);
		var result = await writerRepository.UpdateWriter(id, itemDomain);
		return Ok(result);
	}
	#region //WriterAPI
	//Follow or Unfollow User
	[HttpGet("follow-writer-{id:int}/{idFollow:int}")]
	public async Task<IActionResult> FollowWriter([FromRoute] int id, [FromRoute] int idFollow) {
		await writerRepository.FollowWriter(id, idFollow);
		var item = await writerRepository.GetWriterById(idFollow);
		return Ok(item);
	}
	[HttpGet("writer-{id:int}/follow-story-{idStory:int}")]
	public async Task<IActionResult> FollowStory([FromRoute] int id, [FromRoute] int idStory) {
		await writerRepository.FollowStory(id, idStory);
		var item = await writerRepository.GetWriterById(id);
		return Ok(item);
	}

	//Get list who following user.
	[HttpGet("writer-{id:int}/get-list-follower")]
	public async Task<IActionResult> GetListFollower([FromRoute] int id) {
		var itemList = await writerRepository.GetListFollower(id);
		var listDTO = mapper.Map<List<ReadFollowWriterDTO>>(itemList);
		return Ok(listDTO);
	}
	//Get list who user following
	[HttpGet("writer-{id:int}/get-list-following")]
	public async Task<IActionResult> GetListFollowing([FromRoute] int id) {
		var itemList = await writerRepository.GetListFollowing(id);
		var listDTO = mapper.Map<List<ReadFollowWriterDTO>>(itemList);
		return Ok(listDTO);
	}
	#endregion

	#region//Story API
	[HttpPost("writer-{writerId:int}/add-story-{id:int}")]
	public async Task<IActionResult> AddStoryToWriter([FromRoute] int writerId, [FromRoute] int id) {
		var item = await writerRepository.AddStoryToList(id, writerId);
		return Ok(item);
	}
	[HttpDelete("writer-{writerId:int}/delete-story-{id:int}")]
	public async Task<IActionResult> DeleteStoryFromWriter([FromRoute] int writerId, [FromRoute] int id) {
		var item = await writerRepository.DeleteStoryFromList(id, writerId);
		return Ok(item);
	}
	#endregion

	#region//Post API
	[HttpPost("writer-{writerId:int}/add-post-{id:int}")]
	public async Task<IActionResult> AddPostToWriter([FromRoute] int writerId, [FromRoute] int id) {
		var item = await writerRepository.AddPostToList(id, writerId);
		return Ok(item);
	}
	[HttpDelete("writer-{writerId:int}/delete-post-{id:int}")]
	public async Task<IActionResult> DeletePostToWriter([FromRoute] int writerId, [FromRoute] int id) {
		var item = await writerRepository.DeletePostFromList(id, writerId);
		return Ok(item);
	}
	#endregion
}
