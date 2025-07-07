using EmployeeManager.Server.API.Controllers;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManager.Server.Tests.Controllers
{
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> _mockDepartmentService;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _mockDepartmentService = new Mock<IDepartmentService>();
            _controller = new DepartmentController(_mockDepartmentService.Object);
        }

        [Fact]
        public void Constructor_WithNullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DepartmentController(null));
        }

        [Fact]
        public async Task GetAllDepartments_ReturnsOkResultWithDepartments()
        {
            var expectedDepartments = new List<DepartmentDto>
            {
                new DepartmentDto { DepartmentId = 1, Name = "IT Department", CompanyId = 1 },
                new DepartmentDto { DepartmentId = 2, Name = "HR Department", CompanyId = 1 }
            };

            _mockDepartmentService.Setup(x => x.GetAllDepartmentsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDepartments);

            var result = await _controller.GetAllDepartments();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDepartments = Assert.IsType<List<DepartmentDto>>(okResult.Value);
            Assert.Equal(expectedDepartments.Count, returnedDepartments.Count);
        }

        [Fact]
        public async Task GetDepartmentById_WithValidId_ReturnsOkResult()
        {
            var departmentId = 1;
            var expectedDepartment = new DepartmentDto { DepartmentId = departmentId, Name = "IT Department", CompanyId = 1 };

            _mockDepartmentService.Setup(x => x.GetDepartmentByIdAsync(departmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDepartment);

            var result = await _controller.GetDepartmentById(departmentId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDepartment = Assert.IsType<DepartmentDto>(okResult.Value);
            Assert.Equal(expectedDepartment.DepartmentId, returnedDepartment.DepartmentId);
        }

        [Fact]
        public async Task GetDepartmentById_WithInvalidId_ReturnsNotFound()
        {
            var departmentId = 999;

            _mockDepartmentService.Setup(x => x.GetDepartmentByIdAsync(departmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DepartmentDto)null);

            var result = await _controller.GetDepartmentById(departmentId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetDepartmentByName_WithValidName_ReturnsOkResult()
        {
            var departmentName = "IT Department";
            var expectedDepartment = new DepartmentDto { DepartmentId = 1, Name = departmentName, CompanyId = 1 };

            _mockDepartmentService.Setup(x => x.GetDepartmentByNameAsync(departmentName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedDepartment);

            var result = await _controller.GetDepartmentByName(departmentName);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDepartment = Assert.IsType<DepartmentDto>(okResult.Value);
            Assert.Equal(expectedDepartment.Name, returnedDepartment.Name);
        }

        [Fact]
        public async Task GetDepartmentByName_WithInvalidName_ReturnsNotFound()
        {
            var departmentName = "NonExistentDepartment";

            _mockDepartmentService.Setup(x => x.GetDepartmentByNameAsync(departmentName, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DepartmentDto)null);

            var result = await _controller.GetDepartmentByName(departmentName);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateDepartment_WithValidData_ReturnsCreatedAtAction()
        {
            var createDto = new DepartmentCreateDto
            {
                CompanyId = 1,
                Name = "New Department"
            };

            var createdDepartment = new DepartmentDto
            {
                DepartmentId = 1,
                CompanyId = 1,
                Name = "New Department"
            };

            _mockDepartmentService.Setup(x => x.CreateDepartmentAsync(createDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdDepartment);

            var result = await _controller.CreateDepartment(createDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDepartment = Assert.IsType<DepartmentDto>(createdAtActionResult.Value);
            Assert.Equal(createdDepartment.DepartmentId, returnedDepartment.DepartmentId);
            Assert.Equal(nameof(DepartmentController.GetDepartmentById), createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task CreateDepartment_WithNullData_ReturnsBadRequest()
        {
            var result = await _controller.CreateDepartment(null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateDepartment_WithValidData_ReturnsOkResult()
        {
            var departmentId = 1;
            var updateDto = new DepartmentUpdateDto
            {
                Name = "Updated Department"
            };

            var updatedDepartment = new DepartmentDto
            {
                DepartmentId = departmentId,
                Name = "Updated Department",
                CompanyId = 1
            };

            _mockDepartmentService.Setup(x => x.UpdateDepartmentAsync(departmentId, updateDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedDepartment);

            var result = await _controller.UpdateDepartment(departmentId, updateDto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDepartment = Assert.IsType<DepartmentDto>(okResult.Value);
            Assert.Equal(updatedDepartment.DepartmentId, returnedDepartment.DepartmentId);
        }

        [Fact]
        public async Task UpdateDepartment_WithNullData_ReturnsBadRequest()
        {
            var departmentId = 1;

            var result = await _controller.UpdateDepartment(departmentId, null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateDepartment_WithInvalidId_ReturnsNotFound()
        {
            var departmentId = 999;
            var updateDto = new DepartmentUpdateDto { Name = "Updated Department" };

            _mockDepartmentService.Setup(x => x.UpdateDepartmentAsync(departmentId, updateDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DepartmentDto)null);

            var result = await _controller.UpdateDepartment(departmentId, updateDto);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteDepartment_WithValidId_ReturnsNoContent()
        {
            var departmentId = 1;

            _mockDepartmentService.Setup(x => x.DeleteDepartmentAsync(departmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _controller.DeleteDepartment(departmentId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteDepartment_WithInvalidId_ReturnsNotFound()
        {
            var departmentId = 999;

            _mockDepartmentService.Setup(x => x.DeleteDepartmentAsync(departmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _controller.DeleteDepartment(departmentId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}