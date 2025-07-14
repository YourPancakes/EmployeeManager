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

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        }


        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all departments</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments(CancellationToken cancellationToken = default) =>
            Ok(await _departmentService.GetAllDepartmentsAsync(cancellationToken));

        /// <param name="departmentId">The unique identifier of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, NotFound otherwise</returns>
        [HttpGet("{departmentId:int}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int departmentId, CancellationToken cancellationToken = default)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(departmentId, cancellationToken);
            return department is null ? NotFound() : Ok(department);
        }

        /// <param name="departmentName">The name of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, NotFound otherwise</returns>
        [HttpGet("by-name/{departmentName}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentByName(string departmentName, CancellationToken cancellationToken = default)
        {
            var department = await _departmentService.GetDepartmentByNameAsync(departmentName, cancellationToken);
            return department is null ? NotFound() : Ok(department);
        }

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