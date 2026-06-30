using Ticketing.Api.Contracts.Request;
using FluentValidation;

namespace Ticketing.Api.Validators
{
    public sealed class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
    {
       public CreateTicketRequestValidator() 
        { 
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.UserEmail)
                .NotEmpty().WithMessage("User email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
        }
    }
}
