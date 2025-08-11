using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;

namespace Application.Features.Tasks.Commands;

public sealed class DeleteTaskCommand(IRepository<Domain.Entities.Task.Task> taskRepo) : IUseCase
{
    public async Task<Result<bool,Error>> Handle(DeleteTaskRequest request)
    {

        var task = await taskRepo.Get(new TaskWithAssignedToUsersSpecifications(request.id));

        if (task == null) return Error.NotFound(nameof(DeleteTaskCommand), nameof(request.id), request.id.ToString());

        task.ClearAll();

        taskRepo.Remove(task);

        return true;
    }
}