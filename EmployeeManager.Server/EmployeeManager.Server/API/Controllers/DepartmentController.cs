using EmployeeManager.Server.API.Resources;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Server.API.Controllers
{
    /// <summary>
    /// Controller for managing department-related operations.
    /// Provides endpoints for CRUD operations on departments.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Initializes a new instance of the DepartmentController with required dependencies.
        /// </summary>
        /// <param name="departmentService">The department service for business logic operations</param>
        /// <exception cref="ArgumentNullException">Thrown when department service is null</exception>
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        }

        /// <summary>
        /// Retrieves all departments from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all departments</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments(CancellationToken cancellationToken = default) =>
            Ok(await _departmentService.GetAllDepartmentsAsync(cancellationToken));

        /// <summary>
        /// Retrieves a specific department by its unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, NotFound otherwise</returns>
        [HttpGet("{departmentId:int}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int departmentId, CancellationToken cancellationToken = default)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(departmentId, cancellationToken);
            return department is null ? NotFound() : Ok(department);
        }

        /// <summary>
        /// Retrieves a department by its name.
        /// </summary>
        /// <param name="departmentName">The name of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, NotFound otherwise</returns>
        [HttpGet("by-name/{departmentName}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentByName(string departmentName, CancellationToken cancellationToken = default)
        {
            var department = await _departmentService.GetDepartmentByNameAsync(departmentName, cancellationToken);
            return department is null ? NotFound() : Ok(department);
        }

        /// <summary>
        /// Creates a new department in the system.
        /// </summary>
        /// <param name="departmentCreateDto">The data transfer object containing department information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The created department with assigned identifier</returns>
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] DepartmentCreateDto departmentCreateDto, CancellationToken cancellationToken = default)
        {
            if (departmentCreateDto == null)
            {
                return BadRequest(ValidationMessages.DepartmentDataCannotBeNull);
            }

            var createdDepartment = await _departmentService.CreateDepartmentAsync(departmentCreateDto, cancellationToken);
            return CreatedAtAction(nameof(GetDepartmentById), new { departmentId = createdDepartment.DepartmentId }, createdDepartment);
        }

        /// <summary>
        /// Updates an existing department's information.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to update</param>
        /// <param name="departmentUpdateDto">The data transfer object containing updated department information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated department if successful, NotFound if department not found</returns>
        [HttpPut("{departmentId:int}")]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int departmentId, [FromBody] DepartmentUpdateDto departmentUpdateDto, CancellationToken cancellationToken = default)
        {
            if (departmentUpdateDto == null)
            {
                return BadRequest(ValidationMessages.DepartmentDataCannotBeNull);
            }

            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(departmentId, departmentUpdateDto, cancellationToken);
            return updatedDepartment is null ? NotFound() : Ok(updatedDepartment);
        }

        /// <summary>
        /// Deletes a department from the system by its unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>NoContent if successful, NotFound if department not found</returns>
        [HttpDelete("{departmentId:int}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId, CancellationToken cancellationToken = default)
        {
            var isDeleted = await _departmentService.DeleteDepartmentAsync(departmentId, cancellationToken);
            return isDeleted ? NoContent() : NotFound();
        }
    }
}