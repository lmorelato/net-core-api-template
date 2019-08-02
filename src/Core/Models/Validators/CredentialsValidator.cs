using FluentValidation;
using Template.Core.Models.Dtos;

namespace Template.Core.Models.Validators
{
    public class CredentialsValidator : AbstractValidator<CredentialsDto>
    {
        public CredentialsValidator()
        {
            this.RuleFor(m => m.UserName)
                .NotEmpty()
                .EmailAddress();

            this.RuleFor(m => m.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
