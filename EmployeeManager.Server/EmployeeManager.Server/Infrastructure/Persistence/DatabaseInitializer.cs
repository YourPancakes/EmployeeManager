using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Infrastructure.Persistence
{
    /// <summary>
    /// Hosted service that ensures the database is created, all migrations are applied,
    /// and initial data is seeded when the application starts up.
    /// </summary>
    public class DatabaseInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializer> _logger;

        /// <param name="serviceProvider">The application's service provider.</param>
        /// <param name="logger">The logger instance for logging operations.</param>
        public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Starts the database initialization process, applying all migrations and seeding data.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting database initialization...");

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EmployeeManagerDbContext>();

            try
            {
                _logger.LogInformation("Applying migrations...");
                await context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Migrations applied successfully");

                _logger.LogInformation("Starting database seeding...");
                await DbSeeder.SeedAsync(scope.ServiceProvider);
                _logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database initialization");
                throw;
            }
        }

        /// <summary>
        /// Called when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A completed task.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database initialization completed");
            return Task.CompletedTask;
        }
    }
}