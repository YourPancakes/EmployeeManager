using System.Resources;

namespace EmployeeManager.Server.API.Resources
{
    /// <summary>
    /// Strongly-typed resource class for validation messages displayed to users.
    /// </summary>
    public static class ValidationMessages
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(
            "EmployeeManager.Server.API.Resources.ValidationMessages",
            typeof(ValidationMessages).Assembly);

        /// <summary>
        /// Gets the error message for when employee data is null.
        /// </summary>
        public static string EmployeeDataCannotBeNull =>
            ResourceManager.GetString("EmployeeDataCannotBeNull") ?? "Employee data cannot be null";

        /// <summary>
        /// Gets the error message for when department data is null.
        /// </summary>
        public static string DepartmentDataCannotBeNull =>
            ResourceManager.GetString("DepartmentDataCannotBeNull") ?? "Department data cannot be null";

        /// <summary>
        /// Gets the error message for when salary update validation fails.
        /// </summary>
        public static string SalaryUpdateValidationFailed =>
            ResourceManager.GetString("SalaryUpdateValidationFailed") ?? "Salary update validation failed";

        /// <summary>
        /// Gets the validation message for department name requirements.
        /// </summary>
        public static string DepartmentNameRequired =>
            ResourceManager.GetString("DepartmentNameRequired") ?? "Department name is required and must not exceed 100 characters";

        /// <summary>
        /// Gets the validation message for department name pattern.
        /// </summary>
        public static string DepartmentNamePatternInvalid =>
            ResourceManager.GetString("DepartmentNamePatternInvalid") ?? "Department name can only contain letters, spaces, and hyphens";

        /// <summary>
        /// Gets the validation message for company ID requirement.
        /// </summary>
        public static string CompanyIdRequired =>
            ResourceManager.GetString("CompanyIdRequired") ?? "Company ID must be greater than 0";

        /// <summary>
        /// Gets the validation message for employee full name requirements.
        /// </summary>
        public static string FullNameRequired =>
            ResourceManager.GetString("FullNameRequired") ?? "Full name is required and must not exceed 200 characters";

        /// <summary>
        /// Gets the validation message for birth date requirements.
        /// </summary>
        public static string BirthDateInvalid =>
            ResourceManager.GetString("BirthDateInvalid") ?? "Birth date must be in the past and cannot be more than 100 years ago";

        /// <summary>
        /// Gets the validation message for hire date requirements.
        /// </summary>
        public static string HireDateInvalid =>
            ResourceManager.GetString("HireDateInvalid") ?? "Hire date cannot be in the future";

        /// <summary>
        /// Gets the validation message for salary requirements.
        /// </summary>
        public static string SalaryInvalid =>
            ResourceManager.GetString("SalaryInvalid") ?? "Salary must be greater than 0 and cannot exceed 1,000,000";

        /// <summary>
        /// Gets the validation message for department ID requirement.
        /// </summary>
        public static string DepartmentIdRequired =>
            ResourceManager.GetString("DepartmentIdRequired") ?? "Department ID must be greater than 0";

        /// <summary>
        /// Gets the validation message for new salary requirements.
        /// </summary>
        public static string NewSalaryInvalid =>
            ResourceManager.GetString("NewSalaryInvalid") ?? "New salary must be greater than 0 and cannot exceed 1,000,000";

        /// <summary>
        /// Gets the validation message for maximum current salary requirements.
        /// </summary>
        public static string MaximumCurrentSalaryInvalid =>
            ResourceManager.GetString("MaximumCurrentSalaryInvalid") ?? "Maximum current salary must be greater than 0 and cannot exceed 1,000,000";

        /// <summary>
        /// Gets the validation message for salary update logic requirement.
        /// </summary>
        public static string NewSalaryMustBeHigher =>
            ResourceManager.GetString("NewSalaryMustBeHigher") ?? "New salary should be higher than the maximum current salary threshold to avoid salary reductions";

        /// <summary>
        /// Gets the validation message for pagination parameters being null.
        /// </summary>
        public static string PaginationParametersCannotBeNull =>
            ResourceManager.GetString("PaginationParametersCannotBeNull") ?? "Pagination parameters cannot be null";

        /// <summary>
        /// Gets the validation message for invalid page number.
        /// </summary>
        public static string PageNumberInvalid =>
            ResourceManager.GetString("PageNumberInvalid") ?? "Page number must be greater than 0";

        /// <summary>
        /// Gets the validation message for invalid page size.
        /// </summary>
        public static string PageSizeInvalid =>
            ResourceManager.GetString("PageSizeInvalid") ?? "Page size must be between 1 and 50";
    }
}