namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Generic paginated result DTO that can be used for any entity type.
    /// Contains the data items and pagination metadata for a specific page of results.
    /// </summary>
    /// <typeparam name="T">The type of items in the data collection</typeparam>
    public class PaginatedResultDto<T>
    {
        private const int MINIMUM_PAGE_NUMBER = 1;
        private const int MINIMUM_PAGE_SIZE = 1;

        /// <summary>
        /// Gets or sets the collection of items for the current page.
        /// </summary>
        public IEnumerable<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the current page number (1-based).
        /// </summary>
        public int Page { get; set; } = MINIMUM_PAGE_NUMBER;

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; } = MINIMUM_PAGE_SIZE;

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets whether there is a next page available.
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// Gets or sets whether there is a previous page available.
        /// </summary>
        public bool HasPrevious { get; set; }
    }
}