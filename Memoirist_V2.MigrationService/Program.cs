using Memoirist_V2.MigrationService;
using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.WriterService.DataContext;
using Memoirist_V2.YarpGateway.DataContext;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.Services.AddOpenTelemetry().WithTracing(trace => trace.AddSource(nameof(Worker)));
builder.AddNpgsqlDbContext<WriterDbContext>("writerDb");
builder.AddNpgsqlDbContext<AuthenticateDbContext>("authenDb");
builder.AddNpgsqlDbContext<StoryDbContext>("storyDb");
var host = builder.Build();
host.Run();
