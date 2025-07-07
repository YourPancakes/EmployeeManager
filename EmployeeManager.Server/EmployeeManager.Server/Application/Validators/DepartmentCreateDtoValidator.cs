using EmployeeManager.Server.API.Resources;
using EmployeeManager.Server.Application.DTO;
using FluentValidation;

namespace EmployeeManager.Server.Application.Validators
{
    /// <summary>
    /// Validator for DepartmentCreateDto data transfer object.
    /// Ensures that department creation data meets all business rules and constraints.
    /// </summary>
    public class DepartmentCreateDtoValidator : AbstractValidator<DepartmentCreateDto>
    {
        private const int MAXIMUM_NAME_LENGTH = 100;
        private const string NAME_PATTERN = @"^[a-zA-Zа-яА-Я\s\-]+$";

        /// <summary>
        /// Initializes a new instance of the DepartmentCreateDtoValidator and configures validation rules.
        /// </summary>
        public DepartmentCreateDtoValidator()
        {
            ConfigureCompanyIdValidation();
            ConfigureNameValidation();
        }

        /// <summary>
        /// Configures validation rules for the company identifier.
        /// </summary>
        private void ConfigureCompanyIdValidation()
        {
            RuleFor(department => department.CompanyId)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.CompanyIdRequired);
        }

        /// <summary>
        /// Configures validation rules for the department name.
        /// </summary>
        private void ConfigureNameValidation()
        {
            RuleFor(department => department.Name)
                .NotEmpty()
                .MaximumLength(MAXIMUM_NAME_LENGTH)
                .Matches(NAME_PATTERN)
                .WithMessage(ValidationMessages.DepartmentNameRequired);
        }
    }
}