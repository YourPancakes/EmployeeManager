using EmployeeManager.Server.Domain.Entities;

namespace EmployeeManager.Server.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for company data access operations.
    /// Defines the contract for all company-related data access operations.
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// Retrieves the first company from the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The first company if found, null otherwise</returns>
        Task<Company?> GetFirstAsync(CancellationToken cancellationToken = default);
    }
}