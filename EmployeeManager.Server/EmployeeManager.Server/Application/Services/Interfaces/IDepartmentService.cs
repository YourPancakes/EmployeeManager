using EmployeeManager.Server.Application.DTO;

namespace EmployeeManager.Server.Application.Services.Interfaces
{
    /// <summary>
    /// Service interface for department business logic operations.
    /// Defines the contract for all department-related business operations including CRUD operations.
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// Retrieves all departments from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all departments in the system</returns>
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific department by their unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new department in the system.
        /// </summary>
        /// <param name="departmentCreateDto">The data transfer object containing department information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The created department with assigned identifier</returns>
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing department's information.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to update</param>
        /// <param name="departmentUpdateDto">The data transfer object containing updated department information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated department if successful, null if department not found</returns>
        Task<DepartmentDto?> UpdateDepartmentAsync(int departmentId, DepartmentUpdateDto departmentUpdateDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a department from the system by their unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the department was successfully deleted, false if department not found</returns>
        Task<bool> DeleteDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a department by their name.
        /// </summary>
        /// <param name="departmentName">The name of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        Task<DepartmentDto?> GetDepartmentByNameAsync(string departmentName, CancellationToken cancellationToken = default);
    }
}