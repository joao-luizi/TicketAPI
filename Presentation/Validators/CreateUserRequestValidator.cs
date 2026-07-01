using FluentValidation;
using Ticketing.Api.Contracts.Request;

namespace Ticketing.Api.Validators
{

    public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            //username: 3..80
            RuleFor(x => x.username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 80).WithMessage("Username must be between 3 and 80 characters.");
            //email: formatoemail
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
            //fullname
            RuleFor(x => x.fullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Length(3, 160).WithMessage("Full name must be between 3 and 160 characters.");
            //password : >= 8, 1 maiuscula, 1 minuscula, 1 numero, 1 especial
            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password é obrigatória.")
                .MinimumLength(8).WithMessage("Password deve ter pelo menos 8 caracteres.")
                .Matches("[A-Z]").WithMessage("Password deve conter pelo menos uma letra maiúscula.")
                .Matches("[a-z]").WithMessage("Password deve conter pelo menos uma letra minúscula.")
                .Matches("[0-9]").WithMessage("Password deve conter pelo menos um número.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password deve conter pelo menos um caractere especial.");


        }
    }
}
