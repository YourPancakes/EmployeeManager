using EmployeeManager.Server.Domain.Entities;
using EmployeeManager.Server.Infrastructure.Persistence;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for company data access operations.
    /// Provides data access functionality for company-related operations.
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        private readonly EmployeeManagerDbContext _context;
        private readonly ILogger<CompanyRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the CompanyRepository with required dependencies.
        /// </summary>
        /// <param name="context">The database context for data access</param>
        /// <param name="logger">The logger instance for logging operations</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the required dependencies is null</exception>
        public CompanyRepository(EmployeeManagerDbContext context, ILogger<CompanyRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves the first company from the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The first company if found, null otherwise</returns>
        public async Task<Company?> GetFirstAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving first company from database");

            var company = await _context.Companies
                .Include(c => c.Departments)
                .FirstOrDefaultAsync(cancellationToken);

            if (company == null)
            {
                _logger.LogWarning("No company found in database");
            }
            else
            {
                _logger.LogInformation("Successfully retrieved company: {CompanyName}", company.Name);
            }

            return company;
        }
    }
}