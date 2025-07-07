using EmployeeManager.Server.Application.DTO;

namespace EmployeeManager.Server.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for company statistics operations.
    /// Provides methods to retrieve real-time company statistics from the database.
    /// </summary>
    public interface ICompanyStatisticsService
    {
        /// <summary>
        /// Gets real-time company statistics from the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Company statistics with real data from the database</returns>
        Task<CompanyStatisticsDto> GetCompanyStatisticsAsync(CancellationToken cancellationToken = default);
    }
}