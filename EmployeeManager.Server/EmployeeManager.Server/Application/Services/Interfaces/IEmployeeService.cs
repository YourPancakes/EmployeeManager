using EmployeeManager.Server.Application.DTO;

namespace EmployeeManager.Server.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for employee management operations.
    /// Defines the contract for all employee-related business operations including CRUD operations and specialized queries.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves all employees from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all employees in the system</returns>
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves employees with pagination and optional search support.
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters including page and page size</param>
        /// <param name="searchParameters">Optional search parameters for filtering employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <param name="sortField">Optional sort field for sorting employees</param>
        /// <param name="sortDirection">Optional sort direction for sorting employees</param>
        /// <returns>A paginated result containing employees and pagination metadata</returns>
        Task<PaginatedResultDto<EmployeeDto>> GetEmployeesPaginatedAsync(
            PaginationParametersDto paginationParameters,
            SearchParametersDto? searchParameters = null,
            CancellationToken cancellationToken = default,
            string? sortField = null,
            string? sortDirection = null
        );

        /// <summary>
        /// Retrieves employees with salary higher than the specified threshold.
        /// </summary>
        /// <param name="minimumSalary">Minimum salary threshold to filter employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of employees with salary above the specified threshold</returns>
        Task<IEnumerable<EmployeeDto>> GetEmployeesWithSalaryAboveAsync(decimal minimumSalary, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific employee by their unique identifier.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The employee if found, null otherwise</returns>
        Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new employee in the system.
        /// </summary>
        /// <param name="employeeCreateDto">The data transfer object containing employee information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The created employee with assigned identifier</returns>
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing employee's information.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee to update</param>
        /// <param name="employeeUpdateDto">The data transfer object containing updated employee information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated employee if successful, null if employee not found</returns>
        Task<EmployeeDto?> UpdateEmployeeAsync(int employeeId, EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an employee from the system by their unique identifier.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the employee was successfully deleted, false if employee not found</returns>
        Task<bool> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes all employees older than the specified age.
        /// </summary>
        /// <param name="maximumAge">Maximum age in years for employees to be deleted</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were deleted</returns>
        Task<int> DeleteEmployeesOlderThanAsync(int maximumAge, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates salary to a specific amount for employees with salary below the threshold.
        /// </summary>
        /// <param name="newSalary">The new salary amount to set</param>
        /// <param name="maximumCurrentSalary">Maximum current salary threshold for employees to be updated</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were updated</returns>
        Task<int> UpdateSalaryForLowPaidEmployeesAsync(decimal newSalary, decimal maximumCurrentSalary, CancellationToken cancellationToken = default);
    }
}