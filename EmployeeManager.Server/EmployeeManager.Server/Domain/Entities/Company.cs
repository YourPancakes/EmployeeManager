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


        public int CompanyId { get; set; }

        [Required]
        [StringLength(MAXIMUM_NAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(MINIMUM_FOUNDED_YEAR, MAXIMUM_FOUNDED_YEAR)]
        public int Founded { get; set; }

        [Required]
        [StringLength(MAXIMUM_INDUSTRY_LENGTH)]
        public string Industry { get; set; } = string.Empty;

        [StringLength(MAXIMUM_DESCRIPTION_LENGTH)]
        public string Description { get; set; } = string.Empty;

        [StringLength(MAXIMUM_HEADQUARTERS_LENGTH)]
        public string Headquarters { get; set; } = string.Empty;

        [StringLength(MAXIMUM_WEBSITE_LENGTH)]
        public string Website { get; set; } = string.Empty;

        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}