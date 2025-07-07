using EmployeeManager.Server.API.Controllers;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManager.Server.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_mockEmployeeService.Object);
        }

        [Fact]
        public void Constructor_WithNullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EmployeeController(null));
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsOkResultWithEmployees()
        {
            var expectedEmployees = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = 1, FullName = "John Smith", Salary = 75000 },
                new EmployeeDto { EmployeeId = 2, FullName = "Sarah Johnson", Salary = 82000 }
            };

            _mockEmployeeService.Setup(x => x.GetAllEmployeesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEmployees);

            var result = await _controller.GetAllEmployees();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployees = Assert.IsType<List<EmployeeDto>>(okResult.Value);
            Assert.Equal(expectedEmployees.Count, returnedEmployees.Count);
        }

        [Fact]
        public async Task GetEmployeeById_WithValidId_ReturnsOkResult()
        {
            var employeeId = 1;
            var expectedEmployee = new EmployeeDto { EmployeeId = employeeId, FullName = "John Smith", Salary = 75000 };

            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEmployee);

            var result = await _controller.GetEmployeeById(employeeId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployee = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal(expectedEmployee.EmployeeId, returnedEmployee.EmployeeId);
        }

        [Fact]
        public async Task GetEmployeeById_WithInvalidId_ReturnsNotFound()
        {
            var employeeId = 999;

            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EmployeeDto)null);

            var result = await _controller.GetEmployeeById(employeeId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEmployee_WithValidData_ReturnsCreatedAtAction()
        {
            var createDto = new EmployeeCreateDto
            {
                DepartmentId = 1,
                FullName = "New Employee",
                BirthDate = new DateTime(1990, 1, 1),
                HireDate = new DateTime(2023, 1, 1),
                Salary = 60000
            };

            var createdEmployee = new EmployeeDto
            {
                EmployeeId = 1,
                DepartmentId = 1,
                FullName = "New Employee",
                BirthDate = new DateTime(1990, 1, 1),
                HireDate = new DateTime(2023, 1, 1),
                Salary = 60000
            };

            _mockEmployeeService.Setup(x => x.CreateEmployeeAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdEmployee);

            var result = await _controller.CreateEmployee(createDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedEmployee = Assert.IsType<EmployeeDto>(createdAtActionResult.Value);
            Assert.Equal(createdEmployee.EmployeeId, returnedEmployee.EmployeeId);
            Assert.Equal(nameof(EmployeeController.GetEmployeeById), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task CreateEmployee_WithNullData_ReturnsBadRequest()
        {
            var result = await _controller.CreateEmployee(null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEmployee_WithValidData_ReturnsOkResult()
        {
            var employeeId = 1;
            var updateDto = new EmployeeUpdateDto
            {
                FullName = "Updated Employee",
                Salary = 70000
            };

            var updatedEmployee = new EmployeeDto
            {
                EmployeeId = employeeId,
                FullName = "Updated Employee",
                Salary = 70000
            };

            _mockEmployeeService.Setup(x => x.UpdateEmployeeAsync(employeeId, updateDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedEmployee);

            var result = await _controller.UpdateEmployee(employeeId, updateDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployee = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal(updatedEmployee.EmployeeId, returnedEmployee.EmployeeId);
        }

        [Fact]
        public async Task UpdateEmployee_WithNullData_ReturnsBadRequest()
        {
            var employeeId = 1;

            var result = await _controller.UpdateEmployee(employeeId, null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEmployee_WithInvalidId_ReturnsNotFound()
        {
            var employeeId = 999;
            var updateDto = new EmployeeUpdateDto { FullName = "Updated Employee" };

            _mockEmployeeService.Setup(x => x.UpdateEmployeeAsync(employeeId, updateDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EmployeeDto)null);

            var result = await _controller.UpdateEmployee(employeeId, updateDto);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteEmployee_WithValidId_ReturnsNoContent()
        {
            var employeeId = 1;

            _mockEmployeeService.Setup(x => x.DeleteEmployeeAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _controller.DeleteEmployee(employeeId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WithInvalidId_ReturnsNotFound()
        {
            var employeeId = 999;

            _mockEmployeeService.Setup(x => x.DeleteEmployeeAsync(employeeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _controller.DeleteEmployee(employeeId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetEmployeesBySalaryAbove_ReturnsOkResult()
        {
            var minimumSalary = 70000m;
            var expectedEmployees = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = 1, FullName = "John Smith", Salary = 75000 },
                new EmployeeDto { EmployeeId = 2, FullName = "Sarah Johnson", Salary = 82000 }
            };

            _mockEmployeeService.Setup(x => x.GetEmployeesWithSalaryAboveAsync(minimumSalary, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEmployees);

            var result = await _controller.GetEmployeesBySalaryAbove(minimumSalary);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployees = Assert.IsType<List<EmployeeDto>>(okResult.Value);
            Assert.Equal(expectedEmployees.Count, returnedEmployees.Count);
        }

        [Fact]
        public async Task DeleteEmployeesOlderThan_ReturnsOkResult()
        {
            var maximumAge = 50;
            var deletedCount = 3;

            _mockEmployeeService.Setup(x => x.DeleteEmployeesOlderThanAsync(maximumAge, It.IsAny<CancellationToken>()))
                .ReturnsAsync(deletedCount);

            var result = await _controller.DeleteEmployeesOlderThan(maximumAge);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCount = Assert.IsType<int>(okResult.Value);
            Assert.Equal(deletedCount, returnedCount);
        }

        [Fact]
        public async Task UpdateSalary_WithValidParameters_ReturnsOkResult()
        {
            var newSalary = 75000m;
            var maximumCurrentSalary = 60000m;
            var updatedCount = 2;

            _mockEmployeeService.Setup(x => x.UpdateSalaryForLowPaidEmployeesAsync(newSalary, maximumCurrentSalary, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedCount);

            var result = await _controller.UpdateSalary(newSalary, maximumCurrentSalary);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCount = Assert.IsType<int>(okResult.Value);
            Assert.Equal(updatedCount, returnedCount);
        }

        [Fact]
        public async Task UpdateSalary_WithValidationException_ReturnsBadRequest()
        {
            var newSalary = 75000m;
            var maximumCurrentSalary = 60000m;

            _mockEmployeeService.Setup(x => x.UpdateSalaryForLowPaidEmployeesAsync(newSalary, maximumCurrentSalary, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("Invalid salary"));

            var result = await _controller.UpdateSalary(newSalary, maximumCurrentSalary);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task GetEmployeesPaginated_WithValidParameters_ReturnsOkResult()
        {
            var page = 1;
            var pageSize = 10;
            var department = "IT";
            var fullName = "John";
            var sortField = "FullName";
            var sortDirection = "asc";

            var expectedResult = new PaginatedResultDto<EmployeeDto>
            {
                Data = new List<EmployeeDto>
                {
                    new EmployeeDto { EmployeeId = 1, FullName = "John Smith", Salary = 75000 }
                },
                TotalItems = 1,
                Page = page,
                PageSize = pageSize,
                TotalPages = 1
            };

            _mockEmployeeService.Setup(x => x.GetEmployeesPaginatedAsync(
                It.IsAny<PaginationParametersDto>(),
                It.IsAny<SearchParametersDto>(),
                It.IsAny<CancellationToken>(),
                sortField,
                sortDirection))
                .ReturnsAsync(expectedResult);

            var result = await _controller.GetEmployeesPaginated(page, pageSize, department, fullName, null, null, null, sortField, sortDirection);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResult = Assert.IsType<PaginatedResultDto<EmployeeDto>>(okResult.Value);
            Assert.Equal(expectedResult.TotalItems, returnedResult.TotalItems);
        }

        [Fact]
        public async Task GetEmployeesPaginated_WithNullPaginationParameters_ReturnsBadRequest()
        {
            _mockEmployeeService.Setup(x => x.GetEmployeesPaginatedAsync(
                It.IsAny<PaginationParametersDto>(),
                It.IsAny<SearchParametersDto>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ThrowsAsync(new ArgumentNullException("paginationParameters"));

            var result = await _controller.GetEmployeesPaginated();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task GetEmployeesPaginated_WithInvalidSearchParameters_ReturnsBadRequest()
        {
            _mockEmployeeService.Setup(x => x.GetEmployeesPaginatedAsync(
                It.IsAny<PaginationParametersDto>(),
                It.IsAny<SearchParametersDto>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Invalid search parameters"));

            var result = await _controller.GetEmployeesPaginated();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}