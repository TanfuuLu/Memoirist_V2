using Memoirist_V2.PostService.DataContext;
using Memoirist_V2.PostService.RepoPattern.RabbitMQ;
using Memoirist_V2.PostService.RepoPattern;
using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.StoryService.Mapping;
using Memoirist_V2.StoryService.RepoPattern.RabbitMess;
using Memoirist_V2.StoryService.RepoPattern.StoryRepo;
using Memoirist_V2.WriterService.DataContext;
using Memoirist_V2.WriterService.Mapping;
using Memoirist_V2.WriterService.RepoPattern;
using Memoirist_V2.WriterService.RepoPattern.RabbitMess;
using Memoirist_V2.YarpGateway.DataContext;
using Memoirist_V2.YarpGateway.Mapping;
using Memoirist_V2.YarpGateway.Models;
using Memoirist_V2.YarpGateway.RabbitMess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Memoirist_V2.PostService.Mapping;
using Memoirist_V2.StoryService.RepoPattern.ChapterRepo;
using Microsoft.AspNetCore.Authentication.Facebook;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.AddRabbitMQClient("rabbitMess");
builder.Services.AddScoped<IRabbitYarpRepository, RabbitYarpRepository>();//Yarp Service
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(2));

//Writer Service
builder.Services.AddScoped<IWriterRepository, WriterRepository>();// DI for Writer Service
builder.Services.AddScoped<IRabbitWriterRepository, RabbitWriterRepository>();
builder.AddNpgsqlDbContext<WriterDbContext>("writerDb"); //DB for Writer Service because stupid of Aspire.
builder.Services.AddAutoMapper(typeof(WriterMappingProfile).Assembly);
//Story Service
builder.AddNpgsqlDbContext<StoryDbContext>("storyDb");
builder.Services.AddScoped<IRabbitRepository, RabbitStoryRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddAutoMapper(typeof(StoryMappingProfile).Assembly);
//Post Service
builder.AddNpgsqlDbContext<PostDbContext>("postDb");
builder.Services.AddScoped<IPostRabbitRepository, PostRabbitRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddAutoMapper(typeof(PostMappingProfile).Assembly);

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy")).AddServiceDiscoveryDestinationResolver();

builder.Services.AddAuthorizationBuilder().AddPolicy("AdminPolicy", o => o.RequireRole("Admin"));
builder.AddNpgsqlDbContext<AuthenticateDbContext>("authenDb");
builder.Services.AddIdentity<User, IdentityRole>(options => options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider)
	.AddEntityFrameworkStores<AuthenticateDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders()
	.AddRoles<IdentityRole>();
builder.Services.AddAuthentication(options => {
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultAuthenticateScheme = "Identity.Application";
	options.DefaultSignInScheme = "Identity.Application";
	options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
}).AddCookie(cookie => {
	cookie.Cookie.Name = "token";
}).AddJwtBearer(options => {
	options.TokenValidationParameters = new TokenValidationParameters {
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
	};
	options.Events = new JwtBearerEvents {
		OnMessageReceived = context => {
			context.Token = context.Request.Cookies["token"];
			return Task.CompletedTask;
		}
	};
})
.AddFacebook(facebookOptions => {
	facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
	facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});
builder.Services.Configure<IdentityOptions>(options => {
	options.Password.RequireDigit = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
	options.User.RequireUniqueEmail = false;
});
builder.Services.AddCors(options => {
	options.AddPolicy("AllowAngularApp",
	    policy => policy.WithOrigins("http://localhost:4200") // Angular chạy trên cổng 4200
				  .AllowAnyHeader()
				  .AllowAnyMethod());
});
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();	
app.UseAuthorization();

app.MapControllers();
app.MapReverseProxy();
app.Run();
