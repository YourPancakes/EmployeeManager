using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Server.Domain.Entities
{
    /// <summary>
    /// Represents an employee in the Employee Manager system.
    /// Contains all employee-related information including personal details, employment data, and department association.
    /// </summary>
    public class Employee
    {
        private const int MAXIMUM_FULL_NAME_LENGTH = 200;
        private const double MINIMUM_SALARY = 0;

        /// <summary>
        /// Gets or sets the unique identifier for the employee.
        /// This is the primary key and is auto-generated.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the department this employee belongs to.
        /// This establishes the relationship between Employee and Department entities.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the department this employee belongs to.
        /// This allows for easy access to department information without additional queries.
        /// </summary>
        public Department? Department { get; set; }

        /// <summary>
        /// Gets or sets the full name of the employee.
        /// This should include the complete name (Last Name, First Name, Middle Name if applicable).
        /// Maximum length is 200 characters.
        /// </summary>
        [Required]
        [StringLength(MAXIMUM_FULL_NAME_LENGTH)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the employee.
        /// This is required for age calculations and compliance purposes.
        /// </summary>
        [Required]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the employee was hired.
        /// This is used for calculating employment duration and benefits eligibility.
        /// </summary>
        [Required]
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Gets or sets the current salary of the employee.
        /// This value must be non-negative and is used for payroll and reporting purposes.
        /// </summary>
        [Required]
        [Range(MINIMUM_SALARY, double.MaxValue)]
        public decimal Salary { get; set; }
    }
}