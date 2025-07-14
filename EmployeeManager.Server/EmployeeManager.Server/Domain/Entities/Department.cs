using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Server.Domain.Entities
{
    /// <summary>
    /// Represents a department within the organization.
    /// Contains department information and serves as a grouping mechanism for employees.
    /// </summary>
    public class Department
    {
        private const int MAXIMUM_NAME_LENGTH = 100;

        /// <summary>
        /// Gets or sets the unique identifier for the department.
        /// This is the primary key and is auto-generated.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the company this department belongs to.
        /// This establishes the relationship between Department and Company entities.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the company this department belongs to.
        /// This allows for easy access to company information without additional queries.
        /// </summary>
        public Company? Company { get; set; }

        /// <summary>
        /// Gets or sets the unique name of the department.
        /// This name must be unique within the company and is used for identification.
        /// Maximum length is 100 characters.
        /// </summary>
        [Required]
        [StringLength(MAXIMUM_NAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of employees in this department.
        /// This navigation property allows for easy access to all employees within the department.
        /// </summary>
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}