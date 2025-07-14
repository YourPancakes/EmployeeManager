using EmployeeManager.Server.Domain.Entities;

namespace EmployeeManager.Server.Infrastructure.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for department data access operations.
    /// Defines the contract for all department-related database operations.
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all departments in the database</returns>
        Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <param name="departmentId">The unique identifier of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        Task<Department?> GetByIdAsync(int departmentId, CancellationToken cancellationToken = default);

        /// <param name="departmentName">The name of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        Task<Department?> GetByNameAsync(string departmentName, CancellationToken cancellationToken = default);

        /// <param name="department">The department entity to add</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The added department with assigned identifier</returns>
        Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default);

        /// <param name="department">The department entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated department entity</returns>
        Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken = default);

        /// <param name="departmentId">The unique identifier of the department to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the department was successfully deleted, false if department not found</returns>
        Task<bool> DeleteAsync(int departmentId, CancellationToken cancellationToken = default);
    }
}