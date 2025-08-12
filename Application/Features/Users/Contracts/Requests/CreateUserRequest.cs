using FluentValidation;

namespace Application.Features.Users.Contracts.Requests;

public record CreateUserRequest(string FirstName,string LastName,string Email,string? Image);


public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest> { 
    public CreateUserRequestValidator() {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(u => u.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
