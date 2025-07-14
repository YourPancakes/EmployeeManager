using EmployeeManager.Server.API.Resources;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Server.API.Controllers
{
    /// <summary>
    /// Controller for managing employee-related operations.
    /// Provides endpoints for CRUD operations on employees and specialized queries.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all employees in the system</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees(CancellationToken cancellationToken = default) =>
            Ok(await _employeeService.GetAllEmployeesAsync(cancellationToken));

        /// <summary>
        /// Retrieves employees with pagination and search support.
        /// </summary>
        /// <param name="page">Page number (1-based)</param>
        /// <param name="pageSize">Number of items per page (max 50)</param>
        /// <param name="department">Department name to search for (optional)</param>
        /// <param name="fullName">Full name to search for (optional)</param>
        /// <param name="birthDate">Birth date to search for (optional, format: YYYY-MM-DD)</param>
        /// <param name="hireDate">Hire date to search for (optional, format: YYYY-MM-DD)</param>
        /// <param name="salary">Salary to search for (optional)</param>
        /// <param name="sortField">Field to sort by (optional)</param>
        /// <param name="sortDirection">Direction to sort (optional)</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A paginated result containing employees and pagination metadata</returns>
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginatedResultDto<EmployeeDto>>> GetEmployeesPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? department = null,
            [FromQuery] string? fullName = null,
            [FromQuery] string? birthDate = null,
            [FromQuery] string? hireDate = null,
            [FromQuery] string? salary = null,
            [FromQuery] string? sortField = null,
            [FromQuery] string? sortDirection = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var paginationParameters = new PaginationParametersDto
                {
                    Page = page,
                    PageSize = pageSize
                };

                var searchParameters = new SearchParametersDto
                {
                    Department = department,
                    FullName = fullName,
                    BirthDate = birthDate,
                    HireDate = hireDate,
                    Salary = salary
                };

                searchParameters.Normalize();

                var result = await _employeeService.GetEmployeesPaginatedAsync(paginationParameters, searchParameters, cancellationToken, sortField, sortDirection);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ValidationMessages.PaginationParametersCannotBeNull, Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = "Invalid search parameters", Error = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves employees with salary higher than the specified threshold.
        /// </summary>
        /// <param name="minimumSalary">Minimum salary threshold to filter employees</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of employees with salary above the specified threshold</returns>
        [HttpGet("salary-above/{minimumSalary:decimal}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesBySalaryAbove(decimal minimumSalary, CancellationToken cancellationToken = default) =>
            Ok(await _employeeService.GetEmployeesWithSalaryAboveAsync(minimumSalary, cancellationToken));

        /// <param name="employeeId">The unique identifier of the employee to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The employee if found, NotFound otherwise</returns>
        [HttpGet("{employeeId:int}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int employeeId, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId, cancellationToken);
            return employee is null ? NotFound() : Ok(employee);
        }

        /// <param name="employeeCreateDto">The data transfer object containing employee information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The created employee with assigned identifier</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto, CancellationToken cancellationToken = default)
        {
            if (employeeCreateDto == null)
            {
                return BadRequest(ValidationMessages.EmployeeDataCannotBeNull);
            }

            var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeCreateDto, cancellationToken);
            return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = createdEmployee.EmployeeId }, createdEmployee);
        }

        /// <param name="employeeId">The unique identifier of the employee to update</param>
        /// <param name="employeeUpdateDto">The data transfer object containing updated employee information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated employee if successful, NotFound if employee not found</returns>
        [HttpPut("{employeeId:int}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int employeeId, [FromBody] EmployeeUpdateDto employeeUpdateDto, CancellationToken cancellationToken = default)
        {
            if (employeeUpdateDto == null)
            {
                return BadRequest(ValidationMessages.EmployeeDataCannotBeNull);
            }

            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeId, employeeUpdateDto, cancellationToken);
            return updatedEmployee is null ? NotFound() : Ok(updatedEmployee);
        }

        /// <param name="employeeId">The unique identifier of the employee to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>NoContent if successful, NotFound if employee not found</returns>
        [HttpDelete("{employeeId:int}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId, CancellationToken cancellationToken = default)
        {
            var isDeleted = await _employeeService.DeleteEmployeeAsync(employeeId, cancellationToken);
            return isDeleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes all employees older than the specified maximum age.
        /// </summary>
        /// <param name="maximumAge">Maximum age threshold for employee deletion</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees that were deleted</returns>
        [HttpDelete("older-than/{maximumAge:int}")]
        public async Task<ActionResult<int>> DeleteEmployeesOlderThan(int maximumAge, CancellationToken cancellationToken = default) =>
            Ok(await _employeeService.DeleteEmployeesOlderThanAsync(maximumAge, cancellationToken));

        /// <summary>
        /// Updates salary for employees with current salary below the specified maximum threshold.
        /// </summary>
        /// <param name="newSalary">The new salary to set for qualifying employees</param>
        /// <param name="maximumCurrentSalary">Maximum current salary threshold for qualification</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The number of employees whose salary was updated</returns>
        [HttpPut("update-salary")]
        public async Task<ActionResult<int>> UpdateSalary(
            [FromQuery] decimal newSalary,
            [FromQuery] decimal maximumCurrentSalary,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var updatedCount = await _employeeService.UpdateSalaryForLowPaidEmployeesAsync(newSalary, maximumCurrentSalary, cancellationToken);
                return Ok(updatedCount);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    Message = ValidationMessages.SalaryUpdateValidationFailed,
                    Errors = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage })
                });
            }
        }
    }
}