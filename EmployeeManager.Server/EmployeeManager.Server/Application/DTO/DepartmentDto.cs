namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Data transfer object representing a department in the system.
    /// Contains department information including basic details and company association.
    /// </summary>
    public class DepartmentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the department.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the company this department belongs to.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the company this department belongs to.
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}