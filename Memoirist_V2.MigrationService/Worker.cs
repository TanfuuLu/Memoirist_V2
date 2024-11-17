using Memoirist_V2.StoryService.DataContext;
using Memoirist_V2.WriterService.DataContext;
using Memoirist_V2.YarpGateway.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Memoirist_V2.MigrationService;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService {
	public const string ActivitySourceName = "Migration";
	private static readonly ActivitySource s_activitySource = new(ActivitySourceName);
	protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
		using var activity = s_activitySource.StartActivity("Migration Database", ActivityKind.Client);
		try {
			using var scope = serviceProvider.CreateScope();
			var writerDbContext = scope.ServiceProvider.GetRequiredService<WriterDbContext>();
			var yarpAuthenDbContext = scope.ServiceProvider.GetRequiredService<AuthenticateDbContext>();
			var storyDbContext = scope.ServiceProvider.GetRequiredService<StoryDbContext>();

			await EnsureDatabaseAsync(writerDbContext, cancellationToken);
			await RunMigrationAsync(writerDbContext, cancellationToken);

			await EnsureDatabaseAsync(yarpAuthenDbContext, cancellationToken);
			await RunMigrationAsync(yarpAuthenDbContext, cancellationToken);

			await EnsureDatabaseAsync(storyDbContext, cancellationToken);
			await RunMigrationAsync(storyDbContext, cancellationToken);
		}
		catch(Exception ex) {
			activity?.RecordException(ex);
			throw;
		}
		hostApplicationLifetime.StopApplication();

    }
	private static async Task EnsureDatabaseAsync(DbContext dbcontext, CancellationToken cancellationToken) {
		var dbCreator = dbcontext.GetService<IRelationalDatabaseCreator>();
		var strategy = dbcontext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () => {
			if(!await dbCreator.ExistsAsync(cancellationToken)) {
				{
					await dbCreator.CreateAsync(cancellationToken);
				}
			}
		});
	}
	private static async Task RunMigrationAsync(DbContext dbContext, CancellationToken cancellationToken) {
		var strategy = dbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () => {
			await  using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
			await dbContext.Database.MigrateAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		});
	}
}
