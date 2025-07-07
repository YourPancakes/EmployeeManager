using EmployeeManager.Server.Application.DTO;

namespace EmployeeManager.Server.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for company business logic operations.
    /// Defines the contract for all company-related business operations.
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Retrieves the company information.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The company information</returns>
        Task<CompanyDto> GetCompanyAsync(CancellationToken cancellationToken = default);
    }
}