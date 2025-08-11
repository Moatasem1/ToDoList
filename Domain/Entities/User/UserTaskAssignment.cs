using Domain.Common;
using Domain.Common.Interfaces;

namespace Domain.Entities.User;

public class UserTaskAssignment : IEnitity
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public int StatusId { get; private set; }
    public Entities.Task.TaskStatus Status { get; private set; }


    private UserTaskAssignment() { }

   
    public static Result<UserTaskAssignment, Error> Create(Guid taskId, Guid userId, int statusId)
    {
        var statusValidation = ValidateStatusId(statusId);
        if (statusValidation.IsFailure)
            return statusValidation.Error;

        var userTask = new UserTaskAssignment
        {
            TaskId = taskId,
            UserId = userId,
            StatusId = statusId
        };

        return userTask;
    }

    public Result<bool,Error>Update(int statusId)
    {
        var statusValidation = ValidateStatusId(statusId);
        if (statusValidation.IsFailure)
            return statusValidation.Error;

        StatusId = statusId;
        return true;
    }

    public static Result<bool, Error> ValidateStatusId(int statusId)
    {
        var validStatusIds = Enumeration.GetAll<Entities.Task.TaskStatus>().Select(s=>s.Id).ToList();
        if (!validStatusIds.Contains(statusId))
            return Error.ValueInvalid(nameof(UserTaskAssignment), nameof(StatusId));

        return true;
    }
}