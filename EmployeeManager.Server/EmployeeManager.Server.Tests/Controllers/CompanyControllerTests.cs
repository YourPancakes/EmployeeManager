using EmployeeManager.Server.API.Controllers;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManager.Server.Tests.Controllers
{
    public class CompanyControllerTests
    {
        private readonly Mock<ICompanyService> _mockCompanyService;
        private readonly Mock<ICompanyStatisticsService> _mockStatisticsService;
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _mockCompanyService = new Mock<ICompanyService>();
            _mockStatisticsService = new Mock<ICompanyStatisticsService>();
            _controller = new CompanyController(_mockCompanyService.Object, _mockStatisticsService.Object);
        }

        [Fact]
        public void Constructor_WithNullCompanyService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CompanyController(null, _mockStatisticsService.Object));
        }

        [Fact]
        public void Constructor_WithNullStatisticsService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CompanyController(_mockCompanyService.Object, null));
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithCompany()
        {
            var expectedCompany = new CompanyDto
            {
                CompanyId = 1,
                Name = "Employee Manager Corp",
                Founded = 2024,
                Industry = "Software Development",
                Description = "Leading provider of employee management solutions",
                Headquarters = "Tech City, Innovation State",
                Website = "https://employeemanager.com"
            };

            _mockCompanyService.Setup(x => x.GetCompanyAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCompany);

            var result = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCompany = Assert.IsType<CompanyDto>(okResult.Value);
            Assert.Equal(expectedCompany.CompanyId, returnedCompany.CompanyId);
            Assert.Equal(expectedCompany.Name, returnedCompany.Name);
        }

        [Fact]
        public async Task GetStatistics_ReturnsOkResultWithStatistics()
        {
            var expectedStatistics = new CompanyStatisticsDto
            {
                TotalEmployees = 10,
                Departments = 8,
                FoundedYears = 1,
                ProjectsCompleted = 15,
                ClientSatisfaction = 95.5,
                AnnualRevenue = "$1,000,000"
            };

            _mockStatisticsService.Setup(x => x.GetCompanyStatisticsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedStatistics);

            var result = await _controller.GetStatistics();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedStatistics = Assert.IsType<CompanyStatisticsDto>(okResult.Value);
            Assert.Equal(expectedStatistics.TotalEmployees, returnedStatistics.TotalEmployees);
            Assert.Equal(expectedStatistics.Departments, returnedStatistics.Departments);
            Assert.Equal(expectedStatistics.FoundedYears, returnedStatistics.FoundedYears);
        }
    }
}