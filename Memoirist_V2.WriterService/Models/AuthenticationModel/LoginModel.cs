using System.ComponentModel.DataAnnotations;

namespace Memoirist_V2.WriterService.Models.AuthenticationModel;

public class LoginModel {
	[Required]
	public string? Account { get; set; }
	[Required]
	public string? Password { get; set; }
}
