namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// DTO containing pagination parameters for API requests.
    /// Defines the page number and page size for paginated data retrieval.
    /// </summary>
    public class PaginationParametersDto
    {
        private const int MINIMUM_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 10;
        private const int MAXIMUM_PAGE_SIZE = 50;
        private int _pageSize = DEFAULT_PAGE_SIZE;

        /// <summary>
        /// Gets or sets the page number (1-based).
        /// Default value is 1.
        /// </summary>
        public int Page { get; set; } = MINIMUM_PAGE_NUMBER;

        /// <summary>
        /// Gets or sets the number of items per page.
        /// Default value is 10, maximum is 50.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MAXIMUM_PAGE_SIZE ? MAXIMUM_PAGE_SIZE : value;
        }
    }
}