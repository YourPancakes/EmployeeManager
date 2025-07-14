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

        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        [StringLength(MAXIMUM_NAME_LENGTH)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}