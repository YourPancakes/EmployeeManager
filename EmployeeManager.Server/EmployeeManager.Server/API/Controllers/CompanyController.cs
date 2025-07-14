using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Server.API.Controllers
{
    /// <summary>
    /// Controller for managing company-related operations.
    /// Provides endpoints for retrieving company information and statistics.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/company")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyStatisticsService _statisticsService;

        /// <summary>
        /// Initializes a new instance of the CompanyController with required dependencies.
        /// </summary>
        /// <param name="companyService">The company service for business logic operations</param>
        /// <param name="statisticsService">The statistics service for company statistics</param>
        /// <exception cref="ArgumentNullException">Thrown when any required dependency is null</exception>
        public CompanyController(ICompanyService companyService, ICompanyStatisticsService statisticsService)
        {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        /// <summary>
        /// Retrieves the company information.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The company information if found, NotFound otherwise</returns>
        [HttpGet]
        public async Task<ActionResult<CompanyDto>> Get(CancellationToken cancellationToken = default)
        {
            var company = await _companyService.GetCompanyAsync(cancellationToken);
            return Ok(company);
        }

        /// <summary>
        /// Retrieves company statistics including employee count, department count, and other metrics.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Company statistics with real-time data</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<CompanyStatisticsDto>> GetStatistics(CancellationToken cancellationToken = default)
        {
            var statistics = await _statisticsService.GetCompanyStatisticsAsync(cancellationToken);
            return Ok(statistics);
        }
    }
}