namespace Memoirist_V2.YarpGateway.Models;

public class RegisterUser {
	public string? WriterFullname { get; set; }
	public string? WriterUsername { get; set; }
	public string? WriterAvatar { get; set; }
	public string? Account { get; set; }//email
	public string? Password { get; set; }
	public string? WriterGender { get; set; }
	public string? WriterBirthday { get; set; }
	public string? WriterPhone { get; set; }
	public string? WriterEmail { get; set; }
	public string? Roles { get; set; }
}
