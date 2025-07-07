using EmployeeManager.Server.API.Resources;
using FluentValidation;

namespace EmployeeManager.Server.Application.Validators
{
    /// <summary>
    /// Validator for employee salary update operations.
    /// Ensures that salary update parameters meet all business rules and constraints.
    /// </summary>
    public class EmployeeSalaryUpdateValidator : AbstractValidator<(decimal newSalary, decimal maximumCurrentSalary)>
    {
        /// <summary>
        /// Initializes a new instance of the EmployeeSalaryUpdateValidator and configures validation rules.
        /// </summary>
        public EmployeeSalaryUpdateValidator()
        {
            RuleFor(x => x.newSalary)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.NewSalaryInvalid)
                .LessThanOrEqualTo(ValidationConstants.MAXIMUM_SALARY)
                .WithMessage(ValidationMessages.NewSalaryInvalid);

            RuleFor(x => x.maximumCurrentSalary)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.MaximumCurrentSalaryInvalid)
                .LessThanOrEqualTo(ValidationConstants.MAXIMUM_SALARY)
                .WithMessage(ValidationMessages.MaximumCurrentSalaryInvalid);

            RuleFor(x => x)
                .Must(x => x.newSalary > x.maximumCurrentSalary)
                .WithMessage(ValidationMessages.NewSalaryMustBeHigher)
                .WithName("SalaryUpdateLogic");
        }
    }
}