using EmployeeManager.Server.Domain.Entities;
using EmployeeManager.Server.Infrastructure.Persistence;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for department data access operations.
    /// Provides concrete implementation of department database operations using Entity Framework Core.
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EmployeeManagerDbContext _context;

        /// <summary>
        /// Initializes a new instance of the DepartmentRepository with required dependencies.
        /// </summary>
        /// <param name="context">The database context for data access</param>
        /// <exception cref="ArgumentNullException">Thrown when database context is null</exception>
        public DepartmentRepository(EmployeeManagerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves all departments from the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all departments in the database</returns>
        public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Include(d => d.Company)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a specific department by their unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        public async Task<Department?> GetByIdAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Include(d => d.Company)
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId, cancellationToken);
        }

        /// <summary>
        /// Retrieves a department by their name.
        /// </summary>
        /// <param name="departmentName">The name of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        public async Task<Department?> GetByNameAsync(string departmentName, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Include(d => d.Company)
                .FirstOrDefaultAsync(department => department.Name == departmentName, cancellationToken);
        }

        /// <summary>
        /// Adds a new department to the database.
        /// </summary>
        /// <param name="department">The department entity to add</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The added department with assigned identifier and loaded company</returns>
        /// <exception cref="ArgumentNullException">Thrown when department entity is null</exception>
        public async Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync(cancellationToken);

            // Reload the department with company information
            return await _context.Departments
                .Include(d => d.Company)
                .FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId, cancellationToken) ?? department;
        }

        /// <summary>
        /// Updates an existing department in the database.
        /// </summary>
        /// <param name="department">The department entity with updated information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated department entity with loaded company</returns>
        /// <exception cref="ArgumentNullException">Thrown when department entity is null</exception>
        public async Task<Department> UpdateAsync(Department department, CancellationToken cancellationToken = default)
        {
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            _context.Departments.Update(department);
            await _context.SaveChangesAsync(cancellationToken);

            // Reload the department with company information
            return await _context.Departments
                .Include(d => d.Company)
                .FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId, cancellationToken) ?? department;
        }

        /// <summary>
        /// Deletes a department from the database by their unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the department was successfully deleted, false if department not found</returns>
        public async Task<bool> DeleteAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            var department = await _context.Departments.FindAsync(new object[] { departmentId }, cancellationToken);
            if (department == null)
            {
                return false;
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}