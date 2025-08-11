using FluentValidation;

namespace Application.Features.Tasks.Contracts.Requests;

public record UpdateTaskRequest(Guid TaskId, string Name, List<Guid> AssignToIds);

public class UpdateTaskRequestValidator: AbstractValidator<CreateTaskRequest> {

    public UpdateTaskRequestValidator() {
        RuleFor(t => t.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);
    }
}