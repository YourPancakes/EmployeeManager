namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Data transfer object for creating a new employee in the system.
    /// Contains all required information for employee creation.
    /// </summary>
    public class EmployeeCreateDto
    {
        /// <summary>
        /// Gets or sets the foreign key reference to the department this employee will belong to.
        /// </summary>
        public int DepartmentId { get; set; }

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
        /// Gets or sets the salary of the employee.
        /// </summary>
        public decimal Salary { get; set; }
    }
}