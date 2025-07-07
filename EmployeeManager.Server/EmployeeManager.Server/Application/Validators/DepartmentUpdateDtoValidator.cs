using EmployeeManager.Server.API.Resources;
using EmployeeManager.Server.Application.DTO;
using FluentValidation;

namespace EmployeeManager.Server.Application.Validators
{
    /// <summary>
    /// Validator for DepartmentUpdateDto data transfer object.
    /// Ensures that department update data meets all business rules and constraints.
    /// </summary>
    public class DepartmentUpdateDtoValidator : AbstractValidator<DepartmentUpdateDto>
    {
        private const int MAX_NAME_LENGTH = 100;

        /// <summary>
        /// Initializes a new instance of the DepartmentUpdateDtoValidator and configures validation rules.
        /// </summary>
        public DepartmentUpdateDtoValidator()
        {
            ConfigureCompanyIdValidation();
            ConfigureNameValidation();
        }

        /// <summary>
        /// Configures validation rules for the company identifier.
        /// </summary>
        private void ConfigureCompanyIdValidation() =>
            RuleFor(department => department.CompanyId)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.CompanyIdRequired);

        /// <summary>
        /// Configures validation rules for the department name.
        /// </summary>
        private void ConfigureNameValidation() =>
            RuleFor(department => department.Name)
                .NotEmpty()
                .MaximumLength(MAX_NAME_LENGTH)
                .WithMessage(ValidationMessages.DepartmentNameRequired);
    }
}