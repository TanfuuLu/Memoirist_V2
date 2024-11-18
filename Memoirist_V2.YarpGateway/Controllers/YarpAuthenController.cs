using AutoMapper;
using Memoirist_V2.YarpGateway.Models;
using Memoirist_V2.YarpGateway.RabbitMess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Memoirist_V2.YarpGateway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class YarpAuthenController : ControllerBase {
	private readonly UserManager<User> userManager;
	private readonly SignInManager<User> signInManager;
	private readonly IConfiguration configuration;
	private readonly IMapper mapper;
	private readonly IRabbitYarpRepository rabbitRepository;

	public YarpAuthenController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IMapper mapper, IRabbitYarpRepository rabbitRepository) {
		this.userManager = userManager;
		this.signInManager = signInManager;
		this.configuration = configuration;
		this.mapper = mapper;
		this.rabbitRepository = rabbitRepository;
	}

	[HttpPost("register")]
	public async Task<IActionResult> RegisterUser(RegisterUser registerItem) {
		if(registerItem.Roles == "string") {
			registerItem.Roles = "User";
		}
		User check = await userManager.FindByEmailAsync(registerItem.Account);
		if(check != null) {
			return BadRequest("User is existed");
		}
		User item = new User {
			//Property Authenticate
			Account = registerItem.Account,
			UserName = registerItem.Account,
			Password = registerItem.Password,
			PasswordHash = registerItem.Password,
			Email = registerItem.WriterEmail,
			PhoneNumber = registerItem.WriterPhone,
			//Property for User
			WriterFullname = registerItem.WriterFullname,
			WriterAvatar = "none",
			WriterBio = "string",
			WriterGender = registerItem.WriterGender,
			WriterBirthday = registerItem.WriterBirthday,
			WriterPhone = registerItem.WriterPhone,
			WriterUsername = registerItem.WriterUsername,
			WriterEmail = registerItem.WriterEmail
		};
		var result = await userManager.CreateAsync(item, registerItem.Password);
		if(!result.Succeeded) {
			return BadRequest("register failed");
		}
		if(registerItem.Roles == "Admin") {
			await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(item.Email), "Admin");
		} else if(registerItem.Roles == "User"){
			await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(item.Email), "User");
		}
		//Send item to WriterService with RabbitMQ.
		rabbitRepository.SendUser("UserRegisterMess", item);
		return Ok("Register Successed");
	}
	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUser LoginItem) {
		User user = await userManager.FindByEmailAsync(LoginItem.Account);
		if(user == null) {
			return BadRequest("not found account");
		}
		var result = await signInManager.CheckPasswordSignInAsync(user!, LoginItem.Password, false);
		if(!result.Succeeded) {
			return BadRequest("Wrong password");

		}
		var token = JWTGenerator(user);
		//Send item to WriterService with RabbitMQ.
		rabbitRepository.SendUser("UserLoginMess", user);
		return Ok(token.Result);
	}
	[HttpGet("logout")]
	public async Task<IActionResult> Logout() {
		HttpContext.Response.Cookies.Delete("token");
		return Ok("logouted");
	}

	[NonAction]
	public async Task<dynamic> JWTGenerator(User user) {
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		var userClaims = new[] {
			new Claim(ClaimTypes.Email, user.Account),
			new Claim(ClaimTypes.Role,(await userManager.GetRolesAsync(user)).FirstOrDefault())

		};
		await userManager.AddClaimsAsync(await userManager.FindByEmailAsync(user.Account), userClaims);
		var token = new JwtSecurityToken(
			issuer: configuration["Jwt:Issuer"],
			audience: configuration["Jwt:Audience"],
			claims: userClaims,
			expires: DateTime.Now.AddDays(30),
			signingCredentials: credentials
			);
		var writedToken = new JwtSecurityTokenHandler().WriteToken(token);
		HttpContext.Response.Cookies.Append("token", writedToken,
			new CookieOptions {
				Expires = DateTime.Now.AddDays(30),
				HttpOnly = true,
				Secure = true,
				IsEssential = true,
				SameSite = SameSiteMode.None
			});
		return new { token = writedToken, user = user.WriterUsername };
	}

}
