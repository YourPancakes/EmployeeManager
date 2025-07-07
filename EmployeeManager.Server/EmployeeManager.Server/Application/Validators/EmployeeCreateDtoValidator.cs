using EmployeeManager.Server.API.Resources;
using EmployeeManager.Server.Application.DTO;
using FluentValidation;

namespace EmployeeManager.Server.Application.Validators
{
    /// <summary>
    /// Static class containing validation constants to avoid duplication across validators.
    /// </summary>
    public static class ValidationConstants
    {
        public const int MINIMUM_DEPARTMENT_ID = 1;
        public const int MAXIMUM_FULL_NAME_LENGTH = 200;
        public const int MAXIMUM_AGE_YEARS = 100;
        public const decimal MINIMUM_SALARY = 1;
        public const decimal MAXIMUM_SALARY = 1000000;
    }

    /// <summary>
    /// Validator for EmployeeCreateDto data transfer object.
    /// Ensures that employee creation data meets all business rules and constraints.
    /// </summary>
    public class EmployeeCreateDtoValidator : AbstractValidator<EmployeeCreateDto>
    {
        /// <summary>
        /// Initializes a new instance of the EmployeeCreateDtoValidator and configures validation rules.
        /// </summary>
        public EmployeeCreateDtoValidator()
        {
            ConfigureDepartmentIdValidation();
            ConfigureFullNameValidation();
            ConfigureBirthDateValidation();
            ConfigureHireDateValidation();
            ConfigureSalaryValidation();
        }

        /// <summary>
        /// Configures validation rules for the department identifier.
        /// </summary>
        private void ConfigureDepartmentIdValidation()
        {
            RuleFor(employee => employee.DepartmentId)
                .GreaterThan(ValidationConstants.MINIMUM_DEPARTMENT_ID - 1)
                .WithMessage(ValidationMessages.DepartmentIdRequired);
        }

        /// <summary>
        /// Configures validation rules for the employee's full name.
        /// </summary>
        private void ConfigureFullNameValidation()
        {
            RuleFor(employee => employee.FullName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAXIMUM_FULL_NAME_LENGTH)
                .WithMessage(ValidationMessages.FullNameRequired);
        }

        /// <summary>
        /// Configures validation rules for the employee's birth date.
        /// </summary>
        private void ConfigureBirthDateValidation()
        {
            RuleFor(employee => employee.BirthDate)
                .LessThan(DateTime.Today)
                .WithMessage(ValidationMessages.BirthDateInvalid)
                .GreaterThan(DateTime.Today.AddYears(-ValidationConstants.MAXIMUM_AGE_YEARS))
                .WithMessage(ValidationMessages.BirthDateInvalid);
        }

        /// <summary>
        /// Configures validation rules for the employee's hire date.
        /// </summary>
        private void ConfigureHireDateValidation()
        {
            RuleFor(employee => employee.HireDate)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage(ValidationMessages.HireDateInvalid);
        }

        /// <summary>
        /// Configures validation rules for the employee's salary.
        /// Salary must be greater than 0 and less than or equal to maximum allowed.
        /// </summary>
        private void ConfigureSalaryValidation()
        {
            RuleFor(employee => employee.Salary)
                .GreaterThan(ValidationConstants.MINIMUM_SALARY - 1)
                .WithMessage(ValidationMessages.SalaryInvalid)
                .LessThanOrEqualTo(ValidationConstants.MAXIMUM_SALARY)
                .WithMessage(ValidationMessages.SalaryInvalid);
        }
    }
}