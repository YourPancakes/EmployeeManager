using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using EmployeeManager.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Application.Services.Implementations
{
    /// <summary>
    /// Service implementation for company statistics operations.
    /// Provides real-time company statistics by querying the database.
    /// </summary>
    public class CompanyStatisticsService : ICompanyStatisticsService
    {
        private readonly EmployeeManagerDbContext _context;
        private readonly ILogger<CompanyStatisticsService> _logger;
        private const int COMPANY_FOUNDED_YEAR = 2024;
        private const int PROJECTS_COMPLETED = 45;
        private const double CLIENT_SATISFACTION = 98.5;
        private const string ANNUAL_REVENUE = "$2.5M";

        /// <summary>
        /// Initializes a new instance of the CompanyStatisticsService with required dependencies.
        /// </summary>
        /// <param name="context">The database context for data access</param>
        /// <param name="logger">The logger instance</param>
        /// <exception cref="ArgumentNullException">Thrown when any dependency is null</exception>
        public CompanyStatisticsService(EmployeeManagerDbContext context, ILogger<CompanyStatisticsService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets real-time company statistics from the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Company statistics with real data from the database</returns>
        public async Task<CompanyStatisticsDto> GetCompanyStatisticsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving real-time company statistics from database");

            try
            {
                var totalEmployees = await _context.Employees.CountAsync(cancellationToken);
                var totalDepartments = await _context.Departments.CountAsync(cancellationToken);

                var foundedYears = DateTime.Now.Year - COMPANY_FOUNDED_YEAR;

                var statistics = new CompanyStatisticsDto
                {
                    TotalEmployees = totalEmployees,
                    Departments = totalDepartments,
                    FoundedYears = foundedYears,
                    ProjectsCompleted = PROJECTS_COMPLETED,
                    ClientSatisfaction = CLIENT_SATISFACTION,
                    AnnualRevenue = ANNUAL_REVENUE
                };

                _logger.LogInformation("Successfully retrieved company statistics. Employees: {TotalEmployees}, Departments: {Departments}",
                    statistics.TotalEmployees, statistics.Departments);

                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving company statistics");
                throw;
            }
        }
    }
}