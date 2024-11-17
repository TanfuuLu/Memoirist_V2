using Memoirist_V2.WriterService.DataContext;
using Memoirist_V2.WriterService.Mapping;
using Memoirist_V2.WriterService.RepoPattern;
using Memoirist_V2.WriterService.RepoPattern.RabbitMess;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(WriterMappingProfile));

builder.Services.AddTransient<IRabbitWriterRepository, RabbitWriterRepository>();	
builder.Services.AddTransient<IWriterRepository, WriterRepository>();
builder.AddNpgsqlDbContext<WriterDbContext>("writerDb");

builder.AddRabbitMQClient("rabbitMess");
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
