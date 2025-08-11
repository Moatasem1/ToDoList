using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Commands;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Application.Features.Users.Contracts.Requests;

namespace Application.Features.Tasks.Commands;

public sealed class UpdateTaskCommand(AssignTaskToUsersCommand assignCommand, IRepository<Domain.Entities.Task.Task> taskRepo) : IUseCase
{
    public async Task<Result<bool,Error>> Handle(UpdateTaskRequest request)
    {
        var task = await taskRepo.Get(new TaskSpecifications(request.TaskId));

        if (task == null) return Error.NotFound(nameof(UpdateTaskCommand), nameof(Domain.Entities.Task.Task.Id), request.TaskId.ToString());

        var updateResult = task.Update(request.Name);

        if(updateResult.IsFailure) return updateResult.Error;

        taskRepo.Update(task);

       var assignResult =  await assignCommand.Handle(new AssignTaskToUserRequest(task.Id, request.AssignToIds));
        if (assignResult.IsFailure) return assignResult.Error;

        return true;
    }
}