using FluentValidation;

namespace Application.Features.Tasks.Contracts.Requests;

public record CreateTaskRequest(string Name, List<Guid> AssignToIds);

public class CreateTaskRequestValidator: AbstractValidator<CreateTaskRequest> {

    public CreateTaskRequestValidator() {
        RuleFor(t => t.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);
    }
}