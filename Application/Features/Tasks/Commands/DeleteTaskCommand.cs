using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;

namespace Application.Features.Tasks.Commands;

public sealed class DeleteTaskCommand(IRepository<Domain.Entities.Task.Task> taskRepo) : IUseCase
{
    public async Task<Result<bool,Error>> Handle(Guid userId,Guid taskId)
    {

        var task = await taskRepo.Get(new TaskWithAssignedUsersSpecifications(taskId));

        if (task == null || task.CreatedBy!=userId) return Error.NotFound(nameof(DeleteTaskCommand), nameof(taskId), taskId.ToString());

        task.ClearAll();

        taskRepo.Remove(task);

        return true;
    }
}