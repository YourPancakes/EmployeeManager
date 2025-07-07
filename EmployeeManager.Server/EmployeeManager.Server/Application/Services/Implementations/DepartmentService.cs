using AutoMapper;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using EmployeeManager.Server.Domain.Entities;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;

namespace EmployeeManager.Server.Application.Services.Implementations
{
    /// <summary>
    /// Service implementation for department business logic operations.
    /// Provides business logic for all department-related operations including CRUD operations.
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        /// <summary>
        /// Initializes a new instance of the DepartmentService with required dependencies.
        /// </summary>
        /// <param name="departmentRepository">The department repository for data access</param>
        /// <param name="mapper">The AutoMapper instance for object mapping</param>
        /// <param name="logger">The logger instance for logging operations</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the required dependencies is null</exception>
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all departments from the system.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>A collection of all departments in the system</returns>
        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all departments from the system");

            var departments = await _departmentRepository.GetAllAsync(cancellationToken);
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

            _logger.LogInformation("Successfully retrieved {DepartmentCount} departments", departmentDtos.Count());
            return departmentDtos;
        }

        /// <summary>
        /// Retrieves a specific department by their unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        /// <exception cref="ArgumentException">Thrown when department ID is not positive</exception>
        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            if (departmentId <= 0)
            {
                throw new ArgumentException("Department ID must be positive", nameof(departmentId));
            }

            _logger.LogInformation("Retrieving department with ID {DepartmentId}", departmentId);

            var department = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
            var departmentDto = department != null ? _mapper.Map<DepartmentDto>(department) : null;

            if (departmentDto == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", departmentId);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved department with ID {DepartmentId}", departmentId);
            }

            return departmentDto;
        }

        /// <summary>
        /// Creates a new department in the system.
        /// </summary>
        /// <param name="departmentCreateDto">The data transfer object containing department information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The created department with assigned identifier</returns>
        /// <exception cref="ArgumentNullException">Thrown when department data is null</exception>
        public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateDto departmentCreateDto, CancellationToken cancellationToken = default)
        {
            if (departmentCreateDto == null)
            {
                throw new ArgumentNullException(nameof(departmentCreateDto));
            }

            _logger.LogInformation("Creating new department: {Name}", departmentCreateDto.Name);

            var department = _mapper.Map<Department>(departmentCreateDto);
            var createdDepartment = await _departmentRepository.AddAsync(department, cancellationToken);
            var departmentDto = _mapper.Map<DepartmentDto>(createdDepartment);

            _logger.LogInformation("Successfully created department with ID {DepartmentId}: {Name}", departmentDto.DepartmentId, departmentDto.Name);
            return departmentDto;
        }

        /// <summary>
        /// Updates an existing department's information.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to update</param>
        /// <param name="departmentUpdateDto">The data transfer object containing updated department information</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The updated department if successful, null if department not found</returns>
        /// <exception cref="ArgumentException">Thrown when department ID is not positive</exception>
        /// <exception cref="ArgumentNullException">Thrown when department data is null</exception>
        public async Task<DepartmentDto?> UpdateDepartmentAsync(int departmentId, DepartmentUpdateDto departmentUpdateDto, CancellationToken cancellationToken = default)
        {
            if (departmentId <= 0)
            {
                throw new ArgumentException("Department ID must be positive", nameof(departmentId));
            }

            if (departmentUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(departmentUpdateDto));
            }

            _logger.LogInformation("Updating department with ID {DepartmentId}", departmentId);

            var existingDepartment = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found for update", departmentId);
                return null;
            }

            var updatedDepartmentData = _mapper.Map<Department>(departmentUpdateDto);
            existingDepartment.Name = updatedDepartmentData.Name;

            var updatedDepartment = await _departmentRepository.UpdateAsync(existingDepartment, cancellationToken);
            var departmentDto = _mapper.Map<DepartmentDto>(updatedDepartment);

            _logger.LogInformation("Successfully updated department with ID {DepartmentId}: {Name}", departmentDto.DepartmentId, departmentDto.Name);
            return departmentDto;
        }

        /// <summary>
        /// Deletes a department from the system by their unique identifier.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department to delete</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>True if the department was successfully deleted, false if department not found</returns>
        /// <exception cref="ArgumentException">Thrown when department ID is not positive</exception>
        public async Task<bool> DeleteDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            if (departmentId <= 0)
            {
                throw new ArgumentException("Department ID must be positive", nameof(departmentId));
            }

            _logger.LogInformation("Deleting department with ID {DepartmentId}", departmentId);

            var isDeleted = await _departmentRepository.DeleteAsync(departmentId, cancellationToken);

            if (isDeleted)
            {
                _logger.LogInformation("Successfully deleted department with ID {DepartmentId}", departmentId);
            }
            else
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found for deletion", departmentId);
            }

            return isDeleted;
        }

        /// <summary>
        /// Retrieves a department by their name.
        /// </summary>
        /// <param name="departmentName">The name of the department to retrieve</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The department if found, null otherwise</returns>
        /// <exception cref="ArgumentException">Thrown when department name is null or empty</exception>
        public async Task<DepartmentDto?> GetDepartmentByNameAsync(string departmentName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                throw new ArgumentException("Department name cannot be null or empty", nameof(departmentName));
            }

            _logger.LogInformation("Retrieving department by name: {DepartmentName}", departmentName);

            var department = await _departmentRepository.GetByNameAsync(departmentName, cancellationToken);
            var departmentDto = department != null ? _mapper.Map<DepartmentDto>(department) : null;

            if (departmentDto == null)
            {
                _logger.LogWarning("Department with name '{DepartmentName}' not found", departmentName);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved department with name '{DepartmentName}'", departmentName);
            }

            return departmentDto;
        }
    }
}