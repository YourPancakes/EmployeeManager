using AutoMapper;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Implementations;
using EmployeeManager.Server.Domain.Entities;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;
using Moq;

namespace EmployeeManager.Server.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<IDepartmentRepository> _mockDepartmentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _mockDepartmentRepository = new Mock<IDepartmentRepository>();
            _mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<EmployeeService>>();
            var mockValidator = new Mock<EmployeeManager.Server.Application.Validators.EmployeeSalaryUpdateValidator>();
            _service = new EmployeeService(_mockEmployeeRepository.Object, _mockMapper.Object, mockLogger.Object, mockValidator.Object);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsMappedEmployees()
        {
            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FullName = "John Smith", Salary = 75000 },
                new Employee { EmployeeId = 2, FullName = "Sarah Johnson", Salary = 82000 }
            };

            var expectedDtos = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = 1, FullName = "John Smith", Salary = 75000 },
                new EmployeeDto { EmployeeId = 2, FullName = "Sarah Johnson", Salary = 82000 }
            };

            _mockEmployeeRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeDto>>(employees))
                .Returns(expectedDtos);

            var result = await _service.GetAllEmployeesAsync();

            Assert.Equal(expectedDtos, result);
            _mockEmployeeRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WithValidId_ReturnsEmployee()
        {
            var employeeId = 1;
            var employee = new Employee { EmployeeId = employeeId, FullName = "John Smith", Salary = 75000 };
            var expectedDto = new EmployeeDto { EmployeeId = employeeId, FullName = "John Smith", Salary = 75000 };

            _mockEmployeeRepository.Setup(x => x.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            _mockMapper.Setup(x => x.Map<EmployeeDto>(employee))
                .Returns(expectedDto);

            var result = await _service.GetEmployeeByIdAsync(employeeId);

            Assert.Equal(expectedDto, result);
            _mockEmployeeRepository.Verify(x => x.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_WithInvalidId_ReturnsNull()
        {
            var employeeId = 999;

            _mockEmployeeRepository.Setup(x => x.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee)null);

            var result = await _service.GetEmployeeByIdAsync(employeeId);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateEmployeeAsync_WithValidData_ReturnsCreatedEmployee()
        {
            var createDto = new EmployeeCreateDto
            {
                DepartmentId = 1,
                FullName = "New Employee",
                BirthDate = new DateTime(1990, 1, 1),
                HireDate = new DateTime(2023, 1, 1),
                Salary = 60000
            };

            var employee = new Employee
            {
                EmployeeId = 1,
                DepartmentId = 1,
                FullName = "New Employee",
                BirthDate = new DateTime(1990, 1, 1),
                HireDate = new DateTime(2023, 1, 1),
                Salary = 60000
            };

            var expectedDto = new EmployeeDto
            {
                EmployeeId = 1,
                DepartmentId = 1,
                FullName = "New Employee",
                BirthDate = new DateTime(1990, 1, 1),
                HireDate = new DateTime(2023, 1, 1),
                Salary = 60000
            };

            _mockMapper.Setup(x => x.Map<Employee>(createDto))
                .Returns(employee);

            _mockEmployeeRepository.Setup(x => x.AddAsync(employee, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            _mockMapper.Setup(x => x.Map<EmployeeDto>(employee))
                .Returns(expectedDto);

            var result = await _service.CreateEmployeeAsync(createDto);

            Assert.Equal(expectedDto, result);
            _mockEmployeeRepository.Verify(x => x.AddAsync(employee, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithInvalidId_ReturnsNull()
        {
            var employeeId = 999;
            var updateDto = new EmployeeUpdateDto { FullName = "Updated Employee" };

            _mockEmployeeRepository.Setup(x => x.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee)null);

            var result = await _service.UpdateEmployeeAsync(employeeId, updateDto);

            Assert.Null(result);
            _mockEmployeeRepository.Verify(x => x.UpdateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_WithValidId_ReturnsTrue()
        {
            var employeeId = 1;
            var employee = new Employee { EmployeeId = employeeId, FullName = "John Smith" };

            _mockEmployeeRepository.Setup(x => x.GetByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            _mockEmployeeRepository.Setup(x => x.DeleteAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _service.DeleteEmployeeAsync(employeeId);

            Assert.True(result);
            _mockEmployeeRepository.Verify(x => x.DeleteAsync(employeeId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployeesWithSalaryAboveAsync_ReturnsFilteredEmployees()
        {
            var minimumSalary = 70000m;
            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FullName = "John Smith", Salary = 75000 },
                new Employee { EmployeeId = 2, FullName = "Sarah Johnson", Salary = 82000 }
            };

            var expectedDtos = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = 1, FullName = "John Smith", Salary = 75000 },
                new EmployeeDto { EmployeeId = 2, FullName = "Sarah Johnson", Salary = 82000 }
            };

            _mockEmployeeRepository.Setup(x => x.GetBySalaryAboveAsync(minimumSalary, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeDto>>(employees))
                .Returns(expectedDtos);

            var result = await _service.GetEmployeesWithSalaryAboveAsync(minimumSalary);

            Assert.Equal(expectedDtos, result);
            _mockEmployeeRepository.Verify(x => x.GetBySalaryAboveAsync(minimumSalary, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeesOlderThanAsync_ReturnsDeletedCount()
        {
            var maximumAge = 50;
            var deletedCount = 3;

            _mockEmployeeRepository.Setup(x => x.DeleteEmployeesOlderThanAsync(maximumAge, It.IsAny<CancellationToken>()))
                .ReturnsAsync(deletedCount);

            var result = await _service.DeleteEmployeesOlderThanAsync(maximumAge);

            Assert.Equal(deletedCount, result);
            _mockEmployeeRepository.Verify(x => x.DeleteEmployeesOlderThanAsync(maximumAge, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployeesPaginatedAsync_ReturnsPaginatedResult()
        {
            var paginationParameters = new PaginationParametersDto { Page = 1, PageSize = 10 };
            var searchParameters = new SearchParametersDto { FullName = "John" };
            var sortField = "FullName";
            var sortDirection = "asc";

            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FullName = "John Smith", Salary = 75000 }
            };

            var expectedDtos = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = 1, FullName = "John Smith", Salary = 75000 }
            };

            var expectedResult = new PaginatedResultDto<EmployeeDto>
            {
                Data = expectedDtos,
                TotalItems = 1,
                Page = 1,
                PageSize = 10,
                TotalPages = 1
            };

            _mockEmployeeRepository.Setup(x => x.GetFilteredCountAsync(searchParameters, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mockEmployeeRepository.Setup(x => x.GetFilteredPaginatedAsync(searchParameters, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), sortField, sortDirection))
                .ReturnsAsync(employees);

            _mockMapper.Setup(x => x.Map<IEnumerable<EmployeeDto>>(employees))
                .Returns(expectedDtos);

            var result = await _service.GetEmployeesPaginatedAsync(paginationParameters, searchParameters, default, sortField, sortDirection);

            Assert.Equal(expectedResult.TotalItems, result.TotalItems);
            Assert.Equal(expectedResult.Data, result.Data);
        }
    }
}