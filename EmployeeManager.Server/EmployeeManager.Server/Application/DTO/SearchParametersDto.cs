namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// DTO containing search parameters for employee filtering.
    /// Provides safe search functionality with validation and default values.
    /// </summary>
    public class SearchParametersDto
    {
        /// <summary>
        /// Gets or sets the department name to search for.
        /// Can be partial match, case-insensitive.
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// Gets or sets the full name to search for.
        /// Can be partial match, case-insensitive.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the birth date to search for.
        /// Can be null or empty string for no filtering.
        /// </summary>
        public string? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the hire date to search for.
        /// Can be null or empty string for no filtering.
        /// </summary>
        public string? HireDate { get; set; }

        /// <summary>
        /// Gets or sets the salary to search for.
        /// Can be null or empty string for no filtering.
        /// </summary>
        public string? Salary { get; set; }

        /// <summary>
        /// Validates and normalizes search parameters.
        /// Removes leading/trailing whitespace and handles empty values.
        /// </summary>
        public void Normalize()
        {
            Department = string.IsNullOrWhiteSpace(Department) ? null : Department.Trim();
            FullName = string.IsNullOrWhiteSpace(FullName) ? null : FullName.Trim();
            BirthDate = string.IsNullOrWhiteSpace(BirthDate) ? null : BirthDate.Trim();
            HireDate = string.IsNullOrWhiteSpace(HireDate) ? null : HireDate.Trim();
            Salary = string.IsNullOrWhiteSpace(Salary) ? null : Salary.Trim();
        }

        /// <summary>
        /// Checks if any search parameters are provided.
        /// </summary>
        /// <returns>True if at least one search parameter is provided, false otherwise.</returns>
        public bool HasSearchCriteria()
        {
            return !string.IsNullOrEmpty(Department) ||
                   !string.IsNullOrEmpty(FullName) ||
                   !string.IsNullOrEmpty(BirthDate) ||
                   !string.IsNullOrEmpty(HireDate) ||
                   !string.IsNullOrEmpty(Salary);
        }
    }
}