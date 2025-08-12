using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Tasks.Commands;

public sealed class CreateTaskCommand(IReadOnlyRepository<User> userReadOnlyRepo,IRepository<Domain.Entities.Task.Task> taskRepo, IReadOnlyRepository<Domain.Entities.Task.Task> taskReadOnlyRepo) : IUseCase
{
    public async Task<Result<Guid,Error>> Handle(CreateTaskRequest request,Guid userId)
    {
        var canHandel = await CanHandle(request);
        if (canHandel.IsFailure) return canHandel.Error;

        var taskCreationResult = Domain.Entities.Task.Task.Create(request.Name,userId);
        if(taskCreationResult.IsFailure) return taskCreationResult.Error;

        var task = taskCreationResult.Value;

        foreach(var userID in request.AssignToIds)
        {
            var userAssignmentCreationResult = UserTaskAssignment.Create(task.Id, userID, Domain.Entities.Task.TaskStatus.NotStarted.Id);
            if (userAssignmentCreationResult.IsFailure) return userAssignmentCreationResult.Error;

            var userAssignment = userAssignmentCreationResult.Value;

            task.AddUserAssignment(userAssignment);
        }

        await taskRepo.Add(task);

        return task.Id;
    }

    private async Task<Result<bool, Error>> CanHandle(CreateTaskRequest request)
    {
        var taskCount = await taskReadOnlyRepo.Count(new TaskSpecifications(request.Name));

        if(taskCount==1) return Error.ValueAlreadyExists(nameof(CreateTaskCommand),nameof(Domain.Entities.Task.Task.Name),request.Name);

        foreach (var userId in request.AssignToIds)
        {
            var userCount = await userReadOnlyRepo.Count(new UserSpecfications(userId));

            if (userCount == 0) return Error.NotFound(nameof(CreateTaskCommand), nameof(User), userId.ToString());
        }

        return true;
    }
}