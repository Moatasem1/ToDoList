using Application.Features.Tasks.Contracts;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Tasks.Queries;

public sealed class GetUserTasksQuery(IReadOnlyRepository<User> userReadOnlyRepo,IReadOnlyRepository<Domain.Entities.Task.Task> taskReadOnlyRepo,IReadOnlyRepository<UserTaskAssignment> assignmentReadOnlyRepo) : IUseCase
{
    public async Task<Result<List<TaskAssignmentDto>, Error>> Handle(Guid userId)
    {
        var assignments = await assignmentReadOnlyRepo.GetAll(new TaskAssignmentAssignedToUserSpecification(userId));

        if (!assignments.Any())
            return new List<TaskAssignmentDto>();

        var tasksIds = assignments.Select(a=>a.TaskId).ToList();

        var tasks = await taskReadOnlyRepo.GetAll(new TaskWithAssignedUsersSpecifications(tasksIds));

        var userIds = tasks
            .SelectMany(t => t.UserAssigments.Select(ua => ua.UserId)
            .Append(t.CreatedBy))
            .ToHashSet();

        var users = await userReadOnlyRepo.GetAll(new UserSpecfications([..userIds]));
        var usersDic = users.ToDictionary(u => u.Id);

        var result = (from assignment in assignments
                      join task in tasks on assignment.TaskId equals task.Id
                      join user in users on assignment.UserId equals user.Id
                      select new TaskAssignmentDto(
                          task.Id,
                          task.Name,
                          [.. task.UserAssigments.Select(u => GetUser(u.UserId, usersDic))],
                          GetUser(task.CreatedBy, usersDic)
                        )).ToList();

        return result;
    }

    public static UserBasicDetailsDto GetUser(Guid userId, Dictionary<Guid,User>users)
    {
        var user = users[userId];
        return new UserBasicDetailsDto(user.Id, user.FullName, "");
    }
}
