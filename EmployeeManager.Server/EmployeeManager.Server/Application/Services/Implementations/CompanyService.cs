using AutoMapper;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Application.Services.Interfaces;
using EmployeeManager.Server.Infrastructure.Repositories.Interfaces;

namespace EmployeeManager.Server.Application.Services.Implementations
{
    /// <summary>
    /// Service implementation for company business logic operations.
    /// Provides business logic for all company-related operations.
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        /// <summary>
        /// Initializes a new instance of the CompanyService with required dependencies.
        /// </summary>
        /// <param name="companyRepository">The company repository for data access</param>
        /// <param name="mapper">The AutoMapper instance for object mapping</param>
        /// <param name="logger">The logger instance for logging operations</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the required dependencies is null</exception>
        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves the company information.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>The company information</returns>
        public async Task<CompanyDto> GetCompanyAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving company information");

            var company = await _companyRepository.GetFirstAsync(cancellationToken);
            if (company == null)
            {
                _logger.LogWarning("No company found in the database");
                throw new InvalidOperationException("No company found in the database");
            }

            var companyDto = _mapper.Map<CompanyDto>(company);

            _logger.LogInformation("Successfully retrieved company information: {CompanyName}", companyDto.Name);
            return companyDto;
        }
    }
}