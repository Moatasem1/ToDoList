using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Commands;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;

namespace Application.Features.Tasks.Commands;

public sealed class UpdateTaskStatusCommand(IRepository<Domain.Entities.Task.Task> taskRepo) : IUseCase
{
    public async Task<Result<bool,Error>> Handle(UpdateTaskStatusRequest request,Guid userId, Guid taskId)
    {
        var task = await taskRepo.Get(new TaskWithAssignedUsersSpecifications(taskId));

        if (task == null) return Error.NotFound(nameof(UpdateTaskCommand), nameof(Domain.Entities.Task.Task.Id), taskId.ToString());

        var assignedUsersIds = task.UserAssigments.Select(u=>u.UserId).ToHashSet();

        if(!assignedUsersIds.Contains(userId)) return Error.NotFound(nameof(UpdateTaskCommand), nameof(Domain.Entities.Task.Task.Id), taskId.ToString());

        var assignment = task.UserAssigments.First(u=>u.UserId == userId);

        var updationResult = assignment.Update(request.TaskStatus);

        if(updationResult.IsFailure) return updationResult.Error;

        return true;
    }
}