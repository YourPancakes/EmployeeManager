using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Server.Domain.Entities
{
    /// <summary>
    /// Represents a company in the Employee Manager system.
    /// Contains all company-related information including name, industry, and departments.
    /// </summary>
    public class Company
    {
        private const int MAXIMUM_NAME_LENGTH = 200;
        private const int MAXIMUM_INDUSTRY_LENGTH = 100;
        private const int MAXIMUM_DESCRIPTION_LENGTH = 500;
        private const int MAXIMUM_HEADQUARTERS_LENGTH = 200;
        private const int MAXIMUM_WEBSITE_LENGTH = 200;
        private const int MINIMUM_FOUNDED_YEAR = 1800;
        private const int MAXIMUM_FOUNDED_YEAR = 2100;

        /// <summary>
        /// Gets or sets the unique identifier for the company.
        /// This is the primary key and is auto-generated.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// This is the official company name used for identification.
        /// Maximum length is 200 characters.
        /// </summary>
        [Required]
        [StringLength(MAXIMUM_NAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year when the company was founded.
        /// This is used for company age calculations and historical reporting.
        /// </summary>
        [Required]
        [Range(MINIMUM_FOUNDED_YEAR, MAXIMUM_FOUNDED_YEAR)]
        public int Founded { get; set; }

        /// <summary>
        /// Gets or sets the industry sector of the company.
        /// This categorizes the company's business activities.
        /// Maximum length is 100 characters.
        /// </summary>
        [Required]
        [StringLength(MAXIMUM_INDUSTRY_LENGTH)]
        public string Industry { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the company.
        /// This provides additional information about the company's business and activities.
        /// Maximum length is 500 characters.
        /// </summary>
        [StringLength(MAXIMUM_DESCRIPTION_LENGTH)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the headquarters location of the company.
        /// This is the primary business address or location.
        /// Maximum length is 200 characters.
        /// </summary>
        [StringLength(MAXIMUM_HEADQUARTERS_LENGTH)]
        public string Headquarters { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the company website URL.
        /// This provides a link to the company's official website.
        /// Maximum length is 200 characters.
        /// </summary>
        [StringLength(MAXIMUM_WEBSITE_LENGTH)]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of departments in this company.
        /// This navigation property allows for easy access to all departments within the company.
        /// </summary>
        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}