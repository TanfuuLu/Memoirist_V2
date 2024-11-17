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
	#region
	//[HttpGet("get-string")]
	//[Authorize(Roles = "Admin")]
	//public async Task<ActionResult> GetString() {
	//	var writer = await rabbitRepository.ReceiveMessage("sendUserRegister");
	//	return Ok(writer);
	//}
	//[HttpGet("get-all-user")]
	//public async Task<IActionResult> GetListWriter() {
	//	var writerList = await writerRepository.GetListWriters();
	//	return Ok(writerList);
	//}
	#endregion
	//Search user
	[HttpGet("get-writer/{id:int}")]
	public async Task<IActionResult> GetWriterById([FromRoute] int id) {
		var writerTwo = await writerRepository.GetWriterById(id);
		if(writerTwo == null) {
			return NotFound("User not found");
		}
		int writerId = await rabbitRepository.ReceiveInt("WriterIdQueue");
		int storyId = await rabbitRepository.ReceiveInt("StoryIdQueue");
		//if(writerId != 0 && storyId != 0) {
		//	writerRepository.UpdateWriterWhenAddStory(writerId,storyId);
		//}
		//int deleteStoryId = await rabbitRepository.ReceiveInt("DeleteStoryIdQueue");
		//if(deleteStoryId != 0) {
		//	writerRepository.UpdateWriterWhenDeleteStory(id, deleteStoryId);
		//}
		if(writerId == id) {
			var writer = await writerRepository.GetWriterById(writerId);
			rabbitRepository.SendListFollowingStoryIdOfWriter(writer.ListFollowingStoryId, "FollowingStoryIdQueue");
			rabbitRepository.SendListStoryOfWriter(writer.ListStoryId, "StoryOfWriterQueue");
			return Ok(writer);
		} else {
			
			rabbitRepository.SendListFollowingStoryIdOfWriter(writerTwo.ListFollowingStoryId, "FollowingStoryIdQueue");
			rabbitRepository.SendListStoryOfWriter(writerTwo.ListStoryId, "StoryOfWriterQueue");
			return Ok(writerTwo);
		}
	}
	//Get user when login from authenticate
	[HttpGet("writer-login")]
	public async Task<IActionResult> WriterLogin() {
		var writeritem = await writerRepository.GetWriterLogin();
		var writerLogin = await writerRepository.GetWriterById(writeritem.WriterId);
		rabbitRepository.SendListFollowingStoryIdOfWriter(writerLogin.ListFollowingStoryId, "FollowingStoryIdQueue");
		rabbitRepository.SendListStoryOfWriter(writerLogin.ListStoryId, "WriterStoryIdQueue");
		return Ok(writerLogin);
	}
	//Update information of writer
	[HttpPut("writer-update")]
	public async Task<IActionResult> WriterUpdate(int id, UpdateWriterDTO itemDTO) {
		var itemDomain = mapper.Map<Writer>(itemDTO);
		var result = await writerRepository.UpdateWriter(id, itemDomain);
		return Ok(result);
	}
	//Follow or Unfollow User
	[HttpGet("follow-writer-{id:int}/{idFollow:int}")]
	public async Task<IActionResult> FollowWriter([FromRoute]int id,[FromRoute] int idFollow) {
		await writerRepository.FollowWriter(id, idFollow);
		var item = await writerRepository.GetWriterById(id);
		return Ok(item);
	}
	[HttpGet("writer-{id:int}/follow-story-{idStory:int}")]
	public async Task<IActionResult> FollowStory([FromRoute]int id, [FromRoute]int idStory) {
		await writerRepository.FollowStory(id, idStory);
		var item = await writerRepository.GetWriterById(id);
		return Ok(item);
	}

	//Get list who following user.
	[HttpGet("writer-{id:int}/get-list-follower")]
	public async Task<IActionResult> GetListFollower([FromRoute]int id) {
		var itemList = await writerRepository.GetListFollower(id);
		var listDTO = mapper.Map<List<ReadFollowWriterDTO>>(itemList);
		return Ok(listDTO);
	}
	//Get list who user following
	[HttpGet("writer-{id:int}/get-list-following")]
	public async Task<IActionResult> GetListFollowing([FromRoute]int id) {
		var itemList = await writerRepository.GetListFollowing(id);
		var listDTO = mapper.Map<List<ReadFollowWriterDTO>>(itemList);
		return Ok(listDTO);	
	}
	[HttpGet("writer-{id:int}/get-list-following-storyid")]
	public async Task<IActionResult> GetListFollowingStoryId([FromRoute] int id) {
		var item = await writerRepository.GetWriterById(id);
		var listItem = await writerRepository.GetListFollowingStoryId(id);
		return Ok(listItem);
	}
	[HttpGet("writer-{id:int}/get-list-storyid")]
	public async Task<IActionResult> GetListStoryId([FromRoute] int id) {
		var item = await writerRepository.GetWriterById(id);
		var listItem = await writerRepository.GetListStoryOfWriter(id);
		return Ok(listItem);
	}
	[HttpPost("writer-{writerId:int}/add-story-{id:int}")]
	public async Task<IActionResult> AddStoryToWriter([FromRoute]int writerId, [FromRoute]int id) {
		var item = await writerRepository.AddStoryToList(id,writerId);
		return Ok(item);
	}
	[HttpDelete("writer-{writerId:int}/delete-story-{id:int}")]
	public async Task<IActionResult> DeleteStoryFromWriter([FromRoute]int writerId, [FromRoute]int id) {
		var item = await writerRepository.DeleteStoryFromList(id, writerId);
		return Ok(item);	
	}
}
