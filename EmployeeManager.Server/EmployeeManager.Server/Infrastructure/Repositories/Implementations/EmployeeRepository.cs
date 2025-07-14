using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Domain.Entities;
using EmployeeManager.Server.Infrastructure.Persistence;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for employee data access operations.
    /// Provides concrete implementation of employee database operations using Entity Framework Core.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeManagerDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        /// <param name="context">The database context for data access</param>
        /// <param name="logger">The logger instance for logging operations</param>
        /// <exception cref="ArgumentNullException">Thrown when database context is null</exception>
        public EmployeeRepository(EmployeeManagerDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all employees in the database</returns>
        public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all employees with department information");

            return await _context.Employees
                .Include(e => e.Department)
                .ToListAsync(cancellationToken);
        }

        /// <param name="minimumSalary">Minimum salary threshold to filter employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of employees with salary above the specified threshold</returns>
        public async Task<IEnumerable<Employee>> GetBySalaryAboveAsync(decimal minimumSalary, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving employees with salary above {MinimumSalary}", minimumSalary);

            return await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Salary > minimumSalary)
                .ToListAsync(cancellationToken);
        }

        /// <param name="employeeId">The unique identifier of the employee to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The employee if found, null otherwise</returns>
        public async Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving employee with ID {EmployeeId} with department information", employeeId);

            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId, cancellationToken);
        }

        /// <param name="employee">The employee entity to add</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The added employee with assigned identifier and department information</returns>
        /// <exception cref="ArgumentNullException">Thrown when employee entity is null</exception>
        public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _logger.LogInformation("Creating employee {FullName}", employee.FullName);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancellationToken);

            // Reload with department information
            return await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId, cancellationToken) ?? employee;
        }

        /// <param name="employee">The employee entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated employee entity</returns>
        /// <exception cref="ArgumentNullException">Thrown when employee entity is null</exception>
        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _logger.LogInformation("Updating employee {EmployeeId}", employee.EmployeeId);

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);

            return employee;
        }

        /// <param name="employeeId">The unique identifier of the employee to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the employee was successfully deleted, false if employee not found</returns>
        public async Task<bool> DeleteAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting employee {EmployeeId}", employeeId);

            var employee = await _context.Employees.FindAsync(new object[] { employeeId }, cancellationToken);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// Updates salary to a specific amount for employees with salary below the threshold.
        /// </summary>
        /// <param name="newSalary">The new salary amount to set</param>
        /// <param name="maximumCurrentSalary">Maximum current salary threshold for employees to be updated</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were updated</returns>
        public async Task<int> UpdateSalaryForLowPaidEmployeesAsync(decimal newSalary, decimal maximumCurrentSalary, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating salary to {NewSalary} for employees with salary below {MaxSalary}", newSalary, maximumCurrentSalary);

            var employeesToUpdate = await _context.Employees
                .Where(e => e.Salary < maximumCurrentSalary)
                .ToListAsync(cancellationToken);

            var updatedCount = employeesToUpdate.Count;

            if (updatedCount > 0)
            {
                foreach (var employee in employeesToUpdate)
                {
                    employee.Salary = newSalary;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return updatedCount;
        }

        /// <summary>
        /// Deletes all employees older than the specified age.
        /// </summary>
        /// <param name="maximumAge">Maximum age in years for employees to be deleted</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were deleted</returns>
        public async Task<int> DeleteEmployeesOlderThanAsync(int maximumAge, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting employees older than {MaxAge} years", maximumAge);

            var employeesToDelete = await _context.Employees
                .Where(e => EF.Functions.DateDiffYear(e.BirthDate, DateTime.Now) > maximumAge)
                .ToListAsync(cancellationToken);

            var deletedCount = employeesToDelete.Count;

            if (deletedCount > 0)
            {
                _context.Employees.RemoveRange(employeesToDelete);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return deletedCount;
        }

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
        public async Task<IEnumerable<Employee>> GetFilteredPaginatedAsync(
            SearchParametersDto? searchParameters = null,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default,
            string? sortField = null,
            string? sortDirection = null)
        {
            _logger.LogInformation("Retrieving employees with database-level filtering and pagination: Skip={Skip}, Take={Take}, SortField={SortField}, SortDirection={SortDirection}", skip, take, sortField, sortDirection);

            var query = _context.Employees.Include(e => e.Department).AsQueryable();

            if (searchParameters != null)
            {
                query = ApplySearchFilters(query, searchParameters);
            }

            bool descending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);
            string field = sortField?.ToLowerInvariant() ?? string.Empty;
            switch (field)
            {
                case "departmentname":
                    _logger.LogInformation("Sorting by Department.Name {Direction}", descending ? "desc" : "asc");
                    query = descending ? query.OrderByDescending(e => e.Department!.Name) : query.OrderBy(e => e.Department!.Name);
                    break;

                case "fullname":
                    _logger.LogInformation("Sorting by FullName {Direction}", descending ? "desc" : "asc");
                    query = descending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName);
                    break;

                case "birthdate":
                    _logger.LogInformation("Sorting by BirthDate {Direction}", descending ? "desc" : "asc");
                    query = descending ? query.OrderByDescending(e => e.BirthDate) : query.OrderBy(e => e.BirthDate);
                    break;

                case "hiredate":
                    _logger.LogInformation("Sorting by HireDate {Direction}", descending ? "desc" : "asc");
                    query = descending ? query.OrderByDescending(e => e.HireDate) : query.OrderBy(e => e.HireDate);
                    break;

                case "salary":
                    _logger.LogInformation("Sorting by Salary {Direction}", descending ? "desc" : "asc");
                    query = descending ? query.OrderByDescending(e => e.Salary) : query.OrderBy(e => e.Salary);
                    break;

                default:
                    _logger.LogInformation("Sorting by EmployeeId asc (default)");
                    query = query.OrderBy(e => e.EmployeeId);
                    break;
            }

            return await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Counts total number of employees matching the search criteria.
        /// </summary>
        /// <param name="searchParameters">Optional search parameters for filtering employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Total count of employees matching the criteria</returns>
        public async Task<int> GetFilteredCountAsync(SearchParametersDto? searchParameters = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Counting employees with database-level filtering");

            var query = _context.Employees.AsQueryable();

            if (searchParameters != null)
            {
                query = ApplySearchFilters(query, searchParameters);
            }

            return await query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Applies search filters to the query at database level.
        /// </summary>
        /// <param name="query">The base query to apply filters to</param>
        /// <param name="searchParameters">The search parameters to apply</param>
        /// <returns>Filtered query</returns>
        private IQueryable<Employee> ApplySearchFilters(IQueryable<Employee> query, SearchParametersDto searchParameters)
        {
            if (!string.IsNullOrEmpty(searchParameters.Department))
            {
                query = query.Where(e => e.Department != null &&
                    e.Department.Name.Contains(searchParameters.Department));
                _logger.LogInformation("Applied department filter: {Department}", searchParameters.Department);
            }

            if (!string.IsNullOrEmpty(searchParameters.FullName))
            {
                query = query.Where(e => e.FullName.Contains(searchParameters.FullName));
                _logger.LogInformation("Applied full name filter: {FullName}", searchParameters.FullName);
            }

            if (!string.IsNullOrEmpty(searchParameters.BirthDate))
            {
                if (DateTime.TryParse(searchParameters.BirthDate, out var birthDate))
                {
                    query = query.Where(e => e.BirthDate.Date == birthDate.Date);
                    _logger.LogInformation("Applied birth date filter: {BirthDate}", searchParameters.BirthDate);
                }
                else
                {
                    _logger.LogWarning("Invalid birth date format: {BirthDate}", searchParameters.BirthDate);
                }
            }

            if (!string.IsNullOrEmpty(searchParameters.HireDate))
            {
                if (DateTime.TryParse(searchParameters.HireDate, out var hireDate))
                {
                    query = query.Where(e => e.HireDate.Date == hireDate.Date);
                    _logger.LogInformation("Applied hire date filter: {HireDate}", searchParameters.HireDate);
                }
                else
                {
                    _logger.LogWarning("Invalid hire date format: {HireDate}", searchParameters.HireDate);
                }
            }

            if (!string.IsNullOrEmpty(searchParameters.Salary))
            {
                query = query.Where(e => e.Salary.ToString().StartsWith(searchParameters.Salary));
                _logger.LogInformation("Applied salary filter: {Salary}", searchParameters.Salary);
            }

            return query;
        }
    }
}