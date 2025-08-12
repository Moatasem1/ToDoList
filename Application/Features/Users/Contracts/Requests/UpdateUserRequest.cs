using FluentValidation;

namespace Application.Features.Users.Contracts.Requests;

public record UpdateUserRequest(string FirstName,string LastName,string Email,string? Image,bool IsImageUpdate=false);

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
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