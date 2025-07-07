namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Data transfer object representing a company in the system.
    /// Contains all company information including basic details, industry data, and contact information.
    /// </summary>
    public class CompanyDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the company.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year when the company was founded.
        /// </summary>
        public int Founded { get; set; }

        /// <summary>
        /// Gets or sets the industry sector the company operates in.
        /// </summary>
        public string Industry { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the company.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location of the company headquarters.
        /// </summary>
        public string Headquarters { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the company's website URL.
        /// </summary>
        public string Website { get; set; } = string.Empty;
    }
}