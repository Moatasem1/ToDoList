using Domain.Common;
using Domain.Entities.Task;
using FluentValidation;

namespace Application.Features.Tasks.Contracts.Requests;

public record UpdateTaskStatusRequest(int TaskStatus);

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusRequest>
{
    public UpdateTaskStatusValidator()
    {
        var allowedStatusIds = Enumeration.GetAll<Domain.Entities.Task.TaskStatus>().Select(s => s.Id).ToList();

        RuleFor(t => t.TaskStatus)
            .NotEmpty()
            .Must(status => allowedStatusIds.Contains(status));
    }
}
