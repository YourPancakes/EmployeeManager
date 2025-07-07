namespace EmployeeManager.Server.Application.DTO
{
    /// <summary>
    /// Data transfer object representing company statistics and metrics.
    /// Contains aggregated data about the company's performance, workforce, and operational metrics.
    /// </summary>
    public class CompanyStatisticsDto
    {
        /// <summary>
        /// Gets or sets the total number of employees in the company.
        /// </summary>
        public int TotalEmployees { get; set; }

        /// <summary>
        /// Gets or sets the total number of departments in the company.
        /// </summary>
        public int Departments { get; set; }

        /// <summary>
        /// Gets or sets the number of years since the company was founded.
        /// </summary>
        public int FoundedYears { get; set; }

        /// <summary>
        /// Gets or sets the number of projects completed by the company.
        /// </summary>
        public int ProjectsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the client satisfaction rating as a percentage.
        /// </summary>
        public double ClientSatisfaction { get; set; }

        /// <summary>
        /// Gets or sets the annual revenue of the company.
        /// </summary>
        public string AnnualRevenue { get; set; } = string.Empty;
    }
}