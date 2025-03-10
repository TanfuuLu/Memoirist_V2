using Memoirist_V2.PostService.DataContext;
using Memoirist_V2.PostService.Mapping;
using Memoirist_V2.PostService.RepoPattern;
using Memoirist_V2.PostService.RepoPattern.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddNpgsqlDbContext<PostDbContext>("postDb");
builder.AddRabbitMQClient("rabbitMess");
builder.Services.AddAutoMapper(typeof(PostMappingProfile).Assembly);
builder.Services.AddScoped<IPostRabbitRepository, PostRabbitRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

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
