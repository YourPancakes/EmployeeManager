namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Data transfer object representing an employee in the system.
    /// Contains all employee information including personal details, employment data, and department association.
    /// </summary>
    public class EmployeeDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the employee.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the department this employee belongs to.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the department this employee belongs to.
        /// </summary>
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the employee.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the employee.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the employee was hired.
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Gets or sets the current salary of the employee.
        /// </summary>
        public decimal Salary { get; set; }
    }
}