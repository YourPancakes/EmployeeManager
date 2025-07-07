namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// DTO for updating department information
    /// </summary>
    public class DepartmentUpdateDto
    {
        /// <summary>
        /// ID of the company this department belongs to
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Name of the department
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}