namespace Memoirist_V2.PostService.Models.DTO;

public class AddPostDTO {
	public string? PostName { get; set; }
	public int? PostWriterId { get; set; }
	public string? PostWriterAvatar { get; set; }
	public string? PostWriterName { get; set; }
	public string? PostDateTime { get; set; }
	public string? PostContext { get; set; }
}
