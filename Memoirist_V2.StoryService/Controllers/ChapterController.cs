using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using WordRun = DocumentFormat.OpenXml.Wordprocessing.Run;  // Alias cho Run của Word

using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.DTO;
using Memoirist_V2.StoryService.RepoPattern.ChapterRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
	[HttpGet("get-last-{id:int}")]
	public async Task<IActionResult> GetLastChapterOfStory(int id) {
		var result = await chapterRepository.GetLastChapterNumber(id);
		return Ok(result);
	}
	[HttpGet("get-last-chapter-{id:int}")]
	public async Task<IActionResult> GetChapterByNumber(int id) {
		var result = await chapterRepository.GetLastChapterId(id);
		return Ok(result);
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
	[HttpPost("read-word")]
	public async Task<IActionResult> ReadWordAsHtml(IFormFile file) {
		if(file == null || file.Length == 0) {
			return BadRequest("File tải lên không hợp lệ!");
		}

		try {
			var htmlContent = new StringBuilder();

			using(var stream = file.OpenReadStream()) {
				// Mở tệp Word bất đồng bộ sử dụng Open XML SDK
				using(var wordDoc = await Task.Run(() => WordprocessingDocument.Open(stream, false))) {
					// Lấy tất cả các đoạn văn trong tài liệu
					var body = wordDoc.MainDocumentPart.Document.Body;

					foreach(var paragraph in body.Elements<Paragraph>()) {
						var paragraphHtml = new StringBuilder();

						// Kiểm tra nếu đoạn văn không trống
						if(paragraph.InnerText.Trim().Length > 0) {
							// Tạo thẻ <p> cho mỗi đoạn văn (Paragraph)
							paragraphHtml.Append("<p>");

							// Đọc tất cả các Run trong Paragraph
							foreach(var run in paragraph.Elements<WordRun>()) {
								var runHtml = new StringBuilder();

								// Kiểm tra và thêm thẻ <b> nếu chữ in đậm
								if(run.RunProperties != null && run.RunProperties.Bold != null && run.RunProperties.Bold.Val == OnOffValue.FromBoolean(true)) {
									runHtml.Append("<b>");
								}

								// Kiểm tra và thêm thẻ <i> nếu chữ in nghiêng
								if(run.RunProperties != null && run.RunProperties.Italic != null && run.RunProperties.Italic.Val == OnOffValue.FromBoolean(true)) {
									runHtml.Append("<i>");
								}

								// Kiểm tra và thêm thẻ <u> nếu chữ có gạch chân
								if(run.RunProperties != null && run.RunProperties.Underline != null && run.RunProperties.Underline.Val == UnderlineValues.Single) {
									runHtml.Append("<u>");
								}

								// Lấy văn bản từ mỗi Run và thêm vào HTML
								foreach(var text in run.Elements<Text>()) {
									runHtml.Append(text.Text);  // Thêm nội dung văn bản từ Text
								}

								// Đóng các thẻ HTML tương ứng
								if(run.RunProperties?.Underline != null) {
									runHtml.Append("</u>");
								}
								if(run.RunProperties?.Italic != null) {
									runHtml.Append("</i>");
								}
								if(run.RunProperties?.Bold != null) {
									runHtml.Append("</b>");
								}

								paragraphHtml.Append(runHtml.ToString());
							}

							// Đảm bảo rằng mỗi đoạn văn (Paragraph) có thẻ <p> đóng lại
							paragraphHtml.Append("</p>");

							// Thêm đoạn văn đã xử lý vào nội dung HTML tổng
							htmlContent.Append(paragraphHtml.ToString());
						}
					}

					// Thêm thẻ <br> nếu có dấu xuống dòng (nếu cần)
					htmlContent.Replace("\n", "<br>");
				}
			}

			return Ok(new {
				message = "Đọc file thành công!",
				html = htmlContent.ToString()
			});
		}
		catch(Exception ex) {
			return StatusCode(500, new {
				message = "Đã xảy ra lỗi khi xử lý file.",
				error = ex.Message
			});
		}
	}
}
