using EmployeeManager.Server.API.Resources;
using EmployeeManager.Server.Application.DTO;
using FluentValidation;

namespace EmployeeManager.Server.Application.Validators
{
    /// <summary>
    /// Validator for EmployeeUpdateDto data transfer object.
    /// Ensures that employee update data meets all business rules and constraints.
    /// </summary>
    public class EmployeeUpdateDtoValidator : AbstractValidator<EmployeeUpdateDto>
    {
        /// <summary>
        /// Initializes a new instance of the EmployeeUpdateDtoValidator and configures validation rules.
        /// </summary>
        public EmployeeUpdateDtoValidator()
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
        private void ConfigureDepartmentIdValidation() =>
            RuleFor(employee => employee.DepartmentId)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.DepartmentIdRequired);

        /// <summary>
        /// Configures validation rules for the employee's full name.
        /// </summary>
        private void ConfigureFullNameValidation() =>
            RuleFor(employee => employee.FullName)
                .NotEmpty()
                .MaximumLength(ValidationConstants.MAXIMUM_FULL_NAME_LENGTH)
                .WithMessage(ValidationMessages.FullNameRequired);

        /// <summary>
        /// Configures validation rules for the employee's birth date.
        /// </summary>
        private void ConfigureBirthDateValidation() =>
            RuleFor(employee => employee.BirthDate)
                .LessThan(DateTime.Today)
                .WithMessage(ValidationMessages.BirthDateInvalid);

        /// <summary>
        /// Configures validation rules for the employee's hire date.
        /// </summary>
        private void ConfigureHireDateValidation() =>
            RuleFor(employee => employee.HireDate)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage(ValidationMessages.HireDateInvalid);

        /// <summary>
        /// Configures validation rules for the employee's salary.
        /// Salary must be greater than 0 and less than or equal to maximum allowed.
        /// </summary>
        private void ConfigureSalaryValidation() =>
            RuleFor(employee => employee.Salary)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.SalaryInvalid)
                .LessThanOrEqualTo(ValidationConstants.MAXIMUM_SALARY)
                .WithMessage(ValidationMessages.SalaryInvalid);
    }
}