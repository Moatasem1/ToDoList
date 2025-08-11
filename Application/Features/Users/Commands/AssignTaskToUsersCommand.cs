using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts.Requests;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Commands;

public class AssignTaskToUsersCommand(IRepository<Domain.Entities.Task.Task> taskRepo,IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
    /// <summary>
    /// Adds new user assignments and removes any users who are no longer assigned to the task
    /// </summary>
    /// <param name="request">The request containing the task ID and list of user IDs to assign</param>
    /// <returns>
    /// Returns <c>true</c> on success, or an <see cref="Error"/> object on failure
    /// </returns>
    public async Task<Result<bool, Error>> Handle(AssignTaskToUserRequest request)
    {
        
        var canHandel = await CanHandle(request);
        if (canHandel.IsFailure) return canHandel.Error;

        var task = await taskRepo.Get(new TaskWithAssignedToUsersSpecifications(request.TaskId));
        if (task == null) return Error.NotFound(nameof(AssignTaskToUsersCommand), nameof(request.TaskId), request.TaskId.ToString());
        
        var currentUsers = task.UserAssigments.Select(us=>us.UserId).ToList();
        var usersToRemove = currentUsers.Except(request.UserIds).ToList();

        foreach (var userId in request.UserIds)
        {
            if (currentUsers.Contains(userId)) continue;

            var userAssignmentCreationResult = UserTaskAssignment.Create(request.TaskId, userId, Domain.Entities.Task.TaskStatus.NotStarted.Id);
            if (userAssignmentCreationResult.IsFailure) return userAssignmentCreationResult.Error;

            var userAssignment = userAssignmentCreationResult.Value;

            task.AddUserAssignment(userAssignment);
        }

        foreach(var userId in usersToRemove)
            task.RemoveUserAssignment(userId);

        return true;
    }

    private async Task<Result<bool,Error>> CanHandle(AssignTaskToUserRequest request)
    {
        foreach (var userId in request.UserIds) {
            var userCount = await userReadOnlyRepo.Count(new UserSpecfications(userId));

            if (userCount == 0) return Error.NotFound(nameof(CreateTaskCommand), nameof(User), userId.ToString());
        }

        return true;
    }
}
