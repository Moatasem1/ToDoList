using FluentValidation;

namespace Application.Features.Tasks.Contracts.Requests;

public record UpdateTaskRequest(string Name,string? Description, DateTime StartDate,DateTime EndDate, List<Guid> AssignToIds);

public class UpdateTaskRequestValidator: AbstractValidator<UpdateTaskRequest> {

    public UpdateTaskRequestValidator() {
        RuleFor(t => t.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);

        RuleFor(t => t.Description)
           .MinimumLength(3)
           .MaximumLength(1500)
           .When(t => !string.IsNullOrEmpty(t.Description));

        RuleFor(t => t.StartDate)
            .NotEmpty()
            .Must(startDate => startDate > DateTime.Now);

        RuleFor(t => t.EndDate)
           .NotEmpty()
           .Must((request, endDate) => endDate > request.StartDate)
           .WithMessage("EndDate must be later than StartDate.");
    }
}