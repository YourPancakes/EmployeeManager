namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Data transfer object for creating a new department.
    /// Contains all necessary information to create a department record without exposing internal entity structure.
    /// </summary>
    public class DepartmentCreateDto
    {
        /// <summary>
        /// Gets or sets the ID of the company this department belongs to.
        /// This establishes the relationship between the department and its company.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the department.
        /// This name must be unique within the company and is used for identification.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}