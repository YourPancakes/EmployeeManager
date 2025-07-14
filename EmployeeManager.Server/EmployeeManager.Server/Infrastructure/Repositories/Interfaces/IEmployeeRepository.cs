using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Domain.Entities;

namespace EmployeeManager.Server.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for employee data access operations.
    /// Defines the contract for all employee-related database operations.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all employees in the database</returns>
        Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves employees with filtering and pagination support at database level.
        /// </summary>
        /// <param name="searchParameters">Optional search parameters for filtering employees</param>
        /// <param name="skip">Number of items to skip for pagination</param>
        /// <param name="take">Number of items to take for pagination</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <param name="sortField">Optional sort field</param>
        /// <param name="sortDirection">Optional sort direction</param>
        /// <returns>A collection of filtered and paginated employees</returns>
        Task<IEnumerable<Employee>> GetFilteredPaginatedAsync(
            SearchParametersDto? searchParameters = null,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default,
            string? sortField = null,
            string? sortDirection = null
        );

        /// <summary>
        /// Counts total number of employees matching the search criteria.
        /// </summary>
        /// <param name="searchParameters">Optional search parameters for filtering employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Total count of employees matching the criteria</returns>
        Task<int> GetFilteredCountAsync(SearchParametersDto? searchParameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves employees with salary higher than the specified threshold.
        /// </summary>
        /// <param name="minimumSalary">Minimum salary threshold to filter employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of employees with salary above the specified threshold</returns>
        Task<IEnumerable<Employee>> GetBySalaryAboveAsync(decimal minimumSalary, CancellationToken cancellationToken = default);

        /// <param name="employeeId">The unique identifier of the employee to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The employee if found, null otherwise</returns>
        Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken = default);


        /// <param name="employee">The employee entity to add</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The added employee with assigned identifier</returns>
        Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default);


        /// <param name="employee">The employee entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated employee entity</returns>
        Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken = default);


        /// <param name="employeeId">The unique identifier of the employee to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the employee was successfully deleted, false if employee not found</returns>
        Task<bool> DeleteAsync(int employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates salary to a specific amount for employees with salary below the threshold.
        /// </summary>
        /// <param name="newSalary">The new salary amount to set</param>
        /// <param name="maximumCurrentSalary">Maximum current salary threshold for employees to be updated</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were updated</returns>
        Task<int> UpdateSalaryForLowPaidEmployeesAsync(decimal newSalary, decimal maximumCurrentSalary, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all employees older than the specified age.
        /// </summary>
        /// <param name="maximumAge">Maximum age in years for employees to be deleted</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were deleted</returns>
        Task<int> DeleteEmployeesOlderThanAsync(int maximumAge, CancellationToken cancellationToken = default);
    }
}