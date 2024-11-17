using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.WriterService.Models;

public class Writer {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int WriterId { get; set; }
	public string? WriterFullname { get; set; }
	public string? WriterUsername { get; set; }
	public string? WriterAvatar {  get; set; }
	public string? Account {  get; set; } //email
	public string? Password { get; set; }
	public string? WriterBio {  get; set; }
	public string? WriterGender { get; set; }
	public string? WriterBirthday { get; set; }
	public string? WriterPhone { get; set; }
	public string? WriterEmail { get; set; }

	//Post
	public List<int>? ListPostId { get; set; } = new List<int>();

	//Story
	public List<int>? ListFollowingStoryId {  get; set; } = new List<int>();
	public List<int>? ListStoryId {  get; set; } = new List<int>();

	//User
	public List<int>? ListFollower { get; set; } = new List<int>();
	public List<int>? ListFollowing { get; set; } = new List<int>();

}
