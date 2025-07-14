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

        public int EmployeeId { get; set; }

        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        [Required]
        [StringLength(MAXIMUM_FULL_NAME_LENGTH)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        [Range(MINIMUM_SALARY, double.MaxValue)]
        public decimal Salary { get; set; }
    }
}