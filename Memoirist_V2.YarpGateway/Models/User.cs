using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoirist_V2.YarpGateway.Models;

public class User : IdentityUser {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int WriterId { get; set; }
	public string? WriterFullname { get; set; }
	public string? WriterUsername { get; set; }
	public string? WriterAvatar { get; set; }
	public string? Account { get; set; } //email
	public string? Password { get; set; }
	public string? WriterBio { get; set; }
	public string? WriterGender { get; set; }
	public string? WriterBirthday { get; set; }
	public string? WriterPhone { get; set; }
	public string? WriterEmail { get; set; }
}
public class GoogleTokenRequest {
	public string IdToken { get; set; }
}

public class GoogleTokenPayload {
	public string Name { get; set; }
	public string Email { get; set; }
	public string Picture { get; set; }
}