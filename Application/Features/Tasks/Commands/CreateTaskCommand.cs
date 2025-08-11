using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts.Requests;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Tasks.Commands;

public sealed class CreateTaskCommand(IReadOnlyRepository<User> userReadOnlyRepo,IRepository<Domain.Entities.Task.Task> taskRepo) : IUseCase
{
    public async Task<Result<Guid,Error>> Handle(CreateTaskRequest request)
    {
        var canHandel = await CanHandle(request);
        if (canHandel.IsFailure) return canHandel.Error;

        var taskCreationResult = Domain.Entities.Task.Task.Create(request.Name);
        if(taskCreationResult.IsFailure) return taskCreationResult.Error;

        var task = taskCreationResult.Value;

        foreach(var userId in request.AssignToIds)
        {
            var userAssignmentCreationResult = UserTaskAssignment.Create(task.Id, userId, Domain.Entities.Task.TaskStatus.NotStarted.Id);
            if (userAssignmentCreationResult.IsFailure) return userAssignmentCreationResult.Error;

            var userAssignment = userAssignmentCreationResult.Value;

            task.AddUserAssignment(userAssignment);
        }

        await taskRepo.Add(task);

        return task.Id;
    }

    private async Task<Result<bool, Error>> CanHandle(CreateTaskRequest request)
    {
        foreach (var userId in request.AssignToIds)
        {
            var userCount = await userReadOnlyRepo.Count(new UserSpecfications(userId));

            if (userCount == 0) return Error.NotFound(nameof(CreateTaskCommand), nameof(User), userId.ToString());
        }

        return true;
    }
}