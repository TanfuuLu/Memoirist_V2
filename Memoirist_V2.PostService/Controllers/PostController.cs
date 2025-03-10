﻿using AutoMapper;
using Memoirist_V2.PostService.Models;
using Memoirist_V2.PostService.Models.DTO;
using Memoirist_V2.PostService.RepoPattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Memoirist_V2.PostService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase {
	private readonly IPostRepository postRepository;
	private readonly IMapper mapper;
	private readonly IWebHostEnvironment env;
	private readonly string _targetPath = @"E:\MyProject\VSProject\Memoirist_Fe\Memoirist\public\img\post_img\";

	public PostController(IPostRepository postRepository, IMapper mapper, IWebHostEnvironment env) {
		this.postRepository = postRepository;
		this.mapper = mapper;
		this.env = env;
	}

	[HttpGet("get-all-post")]
	public async Task<IActionResult> GetAllPost() {
		var list = await postRepository.GetAllPost();
		return Ok(list);
	}
	[HttpPost("add-post")]
	public async Task<IActionResult> AddPostAPI(AddPostDTO item) {
		var itemDTO = mapper.Map<Post>(item);
		var itemDomain = await postRepository.AddPost(itemDTO);
		return Ok(itemDTO);
	}
	[HttpDelete("delete-post-{id:int}")]
	public async Task<IActionResult> DeletPostAPI([FromRoute] int id) {
		var item = await postRepository.DeletePost(id);
		return Ok(item);
	}
	[HttpPut("update-context-{id:int}")]
	public async Task<IActionResult> UpdateContextApi([FromRoute] int id, string newContext) {
		var item = await postRepository.UpdatePostContext(id, newContext);
		return Ok(item);

	}
	[HttpGet("get-post-{id:int}")]
	public async Task<IActionResult> GetPostById([FromRoute] int id) {
		var item = await postRepository.GetPostById(id);
		return Ok(item);
	}
	[HttpGet("get-list-by-writer-{id:int}")]
	public async Task<IActionResult> GetListPostByWriterId([FromRoute] int id) {
		var listItem = await postRepository.GetListByWriterId(id);
		return Ok(listItem);
	}
	[HttpPost("add-comment")]
	public async Task<IActionResult> AddCommentToPost(AddCommentPost item) {
		if(item.CommentContext == null) {
			return BadRequest("Comment don't have a context");
		}
		var itemDomain = mapper.Map<CommentPost>(item);
		DateTime dateTime = DateTime.Now;
		itemDomain.CommentDate = dateTime.ToString("dd/MM/yyyy");
		itemDomain.CommentLike = 0;
		var itemResult = await postRepository.AddComment(itemDomain);
		return Ok(itemResult);
	}
	[HttpPut("update-comment-{id:int}")]
	public async Task<IActionResult> UpdateCommentToPost(string newContext, int id) {
		var item = await postRepository.UpdateComment(id, newContext);
		return Ok(item);
	}
	[HttpDelete("delete-comment-{id:int}")]
	public async Task<IActionResult> DeleteComment(int id) {
		var item = await postRepository.DeleteCommentPost(id);
		return Ok(item);
	}
	[HttpGet("post-{id:int}/get-list-comment")]
	public async Task<IActionResult> GetListCommentOfPost(int id) {
		var item = await postRepository.GetAllCommentsPost(id);
		return Ok(item);
	}
	[HttpPut("like-post")]
	public async Task<IActionResult> WriterLikePost(int writerId, int postId) {
		var item = await postRepository.LikePost(postId, writerId);
		return Ok(item);
	}
	[HttpPost("upload")]
	public async Task<IActionResult> UploadFile([FromForm] IFormFile[] files) {

		if(files == null || files.Length == 0) {
			return BadRequest("No files uploaded.");
		}

		var fileNames = new List<string>();

		if(!Directory.Exists(_targetPath)) {
			Directory.CreateDirectory(_targetPath);
		}

		foreach(var file in files) {
			var filePath = Path.Combine(_targetPath, file.FileName);

			await using var stream = new FileStream(filePath, FileMode.Create);
			await file.CopyToAsync(stream);

			fileNames.Add(file.FileName); // Chỉ thêm tên file vào danh sách trả về.
		}

		return Ok(fileNames); // Trả về danh sách tên file.
	}

}
