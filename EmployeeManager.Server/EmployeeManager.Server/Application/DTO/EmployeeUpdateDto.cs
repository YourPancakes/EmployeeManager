namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// DTO for updating employee information
    /// </summary>
    public class EmployeeUpdateDto
    {
        /// <summary>
        /// Department ID where the employee works
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Full name of the employee
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Employee's birth date
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Date when the employee was hired
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Employee's salary
        /// </summary>
        public decimal Salary { get; set; }
    }
}