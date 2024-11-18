using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.StoryService.Mapping;
using Memoirist_V2.StoryService.RepoPattern.ChapterRepo;
using Memoirist_V2.StoryService.RepoPattern.RabbitMess;
using Memoirist_V2.StoryService.RepoPattern.StoryRepo;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(StoryMappingProfile));
builder.AddNpgsqlDbContext<StoryDbContext>("storyDb");
builder.AddRabbitMQClient("rabbitMess");


builder.Services.AddScoped<IRabbitRepository, RabbitStoryRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
