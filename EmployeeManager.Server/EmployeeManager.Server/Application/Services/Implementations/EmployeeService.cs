using AutoMapper;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using EmployeeManager.Server.Application.Validators;
using EmployeeManager.Server.Domain.Entities;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;
using FluentValidation;

namespace EmployeeManager.Server.Application.Services.Implementations
{
    /// <summary>
    /// Service implementation for employee management operations.
    /// Provides business logic for all employee-related operations including CRUD operations and specialized queries.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly EmployeeSalaryUpdateValidator _salaryUpdateValidator;

        /// <summary>
        /// Initializes a new instance of the EmployeeService with required dependencies.
        /// </summary>
        /// <param name="employeeRepository">The employee repository for data access</param>
        /// <param name="mapper">The AutoMapper instance for object mapping</param>
        /// <param name="logger">The logger instance for logging operations</param>
        /// <param name="salaryUpdateValidator">The validator for salary update operations</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the required dependencies is null</exception>
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<EmployeeService> logger, EmployeeSalaryUpdateValidator salaryUpdateValidator)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _salaryUpdateValidator = salaryUpdateValidator ?? throw new ArgumentNullException(nameof(salaryUpdateValidator));
        }

        /// <summary>
        /// Retrieves all employees from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all employees in the system</returns>
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all employees from the system");

            var employees = await _employeeRepository.GetAllAsync(cancellationToken);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            _logger.LogInformation("Successfully retrieved {EmployeeCount} employees", employeeDtos.Count());
            return employeeDtos;
        }

        /// <summary>
        /// Retrieves employees with pagination and optional search support.
        /// </summary>
        /// <param name="paginationParameters">Pagination parameters including page and page size</param>
        /// <param name="searchParameters">Optional search parameters for filtering employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <param name="sortField">Optional sort field for sorting employees</param>
        /// <param name="sortDirection">Optional sort direction for sorting employees</param>
        /// <returns>A paginated result containing employees and pagination metadata</returns>
        public async Task<PaginatedResultDto<EmployeeDto>> GetEmployeesPaginatedAsync(
            PaginationParametersDto paginationParameters,
            SearchParametersDto? searchParameters = null,
            CancellationToken cancellationToken = default,
            string? sortField = null,
            string? sortDirection = null)
        {
            if (paginationParameters == null)
            {
                throw new ArgumentNullException(nameof(paginationParameters));
            }

            _logger.LogInformation("Retrieving employees with pagination: Page {Page}, PageSize {PageSize}, SortField={SortField}, SortDirection={SortDirection}",
                paginationParameters.Page, paginationParameters.PageSize, sortField, sortDirection);

            if (searchParameters != null)
            {
                _logger.LogInformation("Search parameters: Department={Department}, FullName={FullName}, BirthDate={BirthDate}, HireDate={HireDate}, Salary={Salary}",
                    searchParameters.Department, searchParameters.FullName, searchParameters.BirthDate, searchParameters.HireDate, searchParameters.Salary);
            }

            var totalItems = await _employeeRepository.GetFilteredCountAsync(searchParameters, cancellationToken);
            _logger.LogInformation("Total employees matching criteria: {TotalItems}", totalItems);

            var totalPages = (int)Math.Ceiling((double)totalItems / paginationParameters.PageSize);
            var skip = (paginationParameters.Page - 1) * paginationParameters.PageSize;

            var employeesForPage = await _employeeRepository.GetFilteredPaginatedAsync(searchParameters, skip, paginationParameters.PageSize, cancellationToken, sortField, sortDirection);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employeesForPage);

            var result = new PaginatedResultDto<EmployeeDto>
            {
                Data = employeeDtos,
                Page = paginationParameters.Page,
                PageSize = paginationParameters.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                HasNext = paginationParameters.Page < totalPages,
                HasPrevious = paginationParameters.Page > 1
            };

            var logMessage = searchParameters != null
                ? "Successfully retrieved {EmployeeCount} filtered employees for page {Page} of {TotalPages}"
                : "Successfully retrieved {EmployeeCount} employees for page {Page} of {TotalPages}";

            _logger.LogInformation(logMessage, employeeDtos.Count(), paginationParameters.Page, totalPages);

            return result;
        }

        /// <summary>
        /// Applies search filters to the employee collection.
        /// </summary>
        /// <param name="employees">The employee collection to filter</param>
        /// <param name="searchParameters">The search parameters to apply</param>
        /// <returns>Filtered employee collection</returns>
        private IEnumerable<Employee> ApplySearchFilters(IEnumerable<Employee> employees, SearchParametersDto searchParameters)
        {
            var filteredEmployees = employees.AsEnumerable();
            var initialCount = filteredEmployees.Count();

            if (!string.IsNullOrEmpty(searchParameters.Department))
            {
                filteredEmployees = filteredEmployees.Where(e =>
                    e.Department != null &&
                    e.Department.Name.Contains(searchParameters.Department, StringComparison.OrdinalIgnoreCase));
                _logger.LogInformation("After department filter '{Department}': {Count} employees", searchParameters.Department, filteredEmployees.Count());
            }

            if (!string.IsNullOrEmpty(searchParameters.FullName))
            {
                filteredEmployees = filteredEmployees.Where(e =>
                    e.FullName.Contains(searchParameters.FullName, StringComparison.OrdinalIgnoreCase));
                _logger.LogInformation("After full name filter '{FullName}': {Count} employees", searchParameters.FullName, filteredEmployees.Count());
            }

            if (!string.IsNullOrEmpty(searchParameters.BirthDate))
            {
                if (DateTime.TryParse(searchParameters.BirthDate, out var birthDate))
                {
                    filteredEmployees = filteredEmployees.Where(e =>
                        e.BirthDate.Date == birthDate.Date);
                    _logger.LogInformation("After birth date filter '{BirthDate}': {Count} employees", searchParameters.BirthDate, filteredEmployees.Count());
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
                    filteredEmployees = filteredEmployees.Where(e =>
                        e.HireDate.Date == hireDate.Date);
                    _logger.LogInformation("After hire date filter '{HireDate}': {Count} employees", searchParameters.HireDate, filteredEmployees.Count());
                }
                else
                {
                    _logger.LogWarning("Invalid hire date format: {HireDate}", searchParameters.HireDate);
                }
            }

            if (!string.IsNullOrEmpty(searchParameters.Salary))
            {
                filteredEmployees = filteredEmployees.Where(e =>
                    e.Salary.ToString().StartsWith(searchParameters.Salary, StringComparison.OrdinalIgnoreCase));
                _logger.LogInformation("After salary filter '{Salary}': {Count} employees", searchParameters.Salary, filteredEmployees.Count());
            }

            _logger.LogInformation("Filtering complete: {InitialCount} -> {FinalCount} employees", initialCount, filteredEmployees.Count());
            return filteredEmployees;
        }

        /// <summary>
        /// Retrieves employees with salary higher than the specified threshold.
        /// </summary>
        /// <param name="minimumSalary">Minimum salary threshold to filter employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of employees with salary above the specified threshold</returns>
        /// <exception cref="ArgumentException">Thrown when minimum salary is negative</exception>
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesWithSalaryAboveAsync(decimal minimumSalary, CancellationToken cancellationToken = default)
        {
            if (minimumSalary < 0)
            {
                throw new ArgumentException("Minimum salary cannot be negative", nameof(minimumSalary));
            }

            _logger.LogInformation("Retrieving employees with salary above {MinimumSalary}", minimumSalary);

            var employees = await _employeeRepository.GetBySalaryAboveAsync(minimumSalary, cancellationToken);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            _logger.LogInformation("Successfully retrieved {EmployeeCount} employees with salary above {MinimumSalary}", employeeDtos.Count(), minimumSalary);
            return employeeDtos;
        }

        /// <summary>
        /// Retrieves a specific employee by their unique identifier.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The employee if found, null otherwise</returns>
        /// <exception cref="ArgumentException">Thrown when employee ID is not positive</exception>
        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Employee ID must be positive", nameof(employeeId));
            }

            _logger.LogInformation("Retrieving employee with ID {EmployeeId}", employeeId);

            var employee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);
            var employeeDto = employee != null ? _mapper.Map<EmployeeDto>(employee) : null;

            if (employeeDto == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", employeeId);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved employee with ID {EmployeeId}", employeeId);
            }

            return employeeDto;
        }

        /// <summary>
        /// Creates a new employee in the system.
        /// </summary>
        /// <param name="employeeCreateDto">The data transfer object containing employee information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The created employee with assigned identifier</returns>
        /// <exception cref="ArgumentNullException">Thrown when employee data is null</exception>
        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            if (employeeCreateDto == null)
            {
                throw new ArgumentNullException(nameof(employeeCreateDto));
            }

            _logger.LogInformation("Creating new employee: {FullName}", employeeCreateDto.FullName);

            var employee = _mapper.Map<Employee>(employeeCreateDto);
            var createdEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);
            var employeeDto = _mapper.Map<EmployeeDto>(createdEmployee);

            _logger.LogInformation("Successfully created employee with ID {EmployeeId}: {FullName}", employeeDto.EmployeeId, employeeDto.FullName);
            return employeeDto;
        }

        /// <summary>
        /// Updates an existing employee's information.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee to update</param>
        /// <param name="employeeUpdateDto">The data transfer object containing updated employee information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated employee if successful, null if employee not found</returns>
        /// <exception cref="ArgumentException">Thrown when employee ID is not positive</exception>
        /// <exception cref="ArgumentNullException">Thrown when employee data is null</exception>
        public async Task<EmployeeDto?> UpdateEmployeeAsync(int employeeId, EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Employee ID must be positive", nameof(employeeId));
            }

            if (employeeUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(employeeUpdateDto));
            }

            _logger.LogInformation("Updating employee with ID {EmployeeId}", employeeId);

            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId, cancellationToken);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found for update", employeeId);
                return null;
            }

            var updatedEmployeeData = _mapper.Map<Employee>(employeeUpdateDto);
            existingEmployee.DepartmentId = updatedEmployeeData.DepartmentId;
            existingEmployee.FullName = updatedEmployeeData.FullName;
            existingEmployee.BirthDate = updatedEmployeeData.BirthDate;
            existingEmployee.HireDate = updatedEmployeeData.HireDate;
            existingEmployee.Salary = updatedEmployeeData.Salary;

            var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee, cancellationToken);
            var employeeDto = _mapper.Map<EmployeeDto>(updatedEmployee);

            _logger.LogInformation("Successfully updated employee with ID {EmployeeId}: {FullName}", employeeDto.EmployeeId, employeeDto.FullName);
            return employeeDto;
        }

        /// <summary>
        /// Deletes an employee from the system by their unique identifier.
        /// </summary>
        /// <param name="employeeId">The unique identifier of the employee to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the employee was successfully deleted, false if employee not found</returns>
        /// <exception cref="ArgumentException">Thrown when employee ID is not positive</exception>
        public async Task<bool> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentException("Employee ID must be positive", nameof(employeeId));
            }

            _logger.LogInformation("Deleting employee with ID {EmployeeId}", employeeId);

            var isDeleted = await _employeeRepository.DeleteAsync(employeeId, cancellationToken);

            if (isDeleted)
            {
                _logger.LogInformation("Successfully deleted employee with ID {EmployeeId}", employeeId);
            }
            else
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion", employeeId);
            }

            return isDeleted;
        }

        /// <summary>
        /// Deletes all employees older than the specified age.
        /// </summary>
        /// <param name="maximumAge">Maximum age in years for employees to be deleted</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were deleted</returns>
        /// <exception cref="ArgumentException">Thrown when maximum age is not positive</exception>
        public async Task<int> DeleteEmployeesOlderThanAsync(int maximumAge, CancellationToken cancellationToken = default)
        {
            if (maximumAge <= 0)
            {
                throw new ArgumentException("Maximum age must be positive", nameof(maximumAge));
            }

            _logger.LogInformation("Deleting employees older than {MaximumAge} years", maximumAge);

            var deletedCount = await _employeeRepository.DeleteEmployeesOlderThanAsync(maximumAge, cancellationToken);

            _logger.LogInformation("Successfully deleted {DeletedCount} employees older than {MaximumAge} years", deletedCount, maximumAge);
            return deletedCount;
        }

        /// <summary>
        /// Updates salary to a specific amount for employees with salary below the threshold.
        /// </summary>
        /// <param name="newSalary">The new salary amount to set</param>
        /// <param name="maximumCurrentSalary">Maximum current salary threshold for employees to be updated</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were updated</returns>
        /// <exception cref="ArgumentException">Thrown when salary parameters are invalid</exception>
        /// <exception cref="ValidationException">Thrown when validation fails</exception>
        public async Task<int> UpdateSalaryForLowPaidEmployeesAsync(decimal newSalary, decimal maximumCurrentSalary, CancellationToken cancellationToken = default)
        {
            var validationResult = await _salaryUpdateValidator.ValidateAsync((newSalary, maximumCurrentSalary), cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Salary update validation failed: {Errors}", errors);
                throw new ValidationException(validationResult.Errors);
            }

            _logger.LogInformation("Updating salary to {NewSalary} for employees with current salary below {MaximumCurrentSalary}", newSalary, maximumCurrentSalary);

            var updatedCount = await _employeeRepository.UpdateSalaryForLowPaidEmployeesAsync(newSalary, maximumCurrentSalary, cancellationToken);

            _logger.LogInformation("Successfully updated salary for {UpdatedCount} employees", updatedCount);
            return updatedCount;
        }
    }
}