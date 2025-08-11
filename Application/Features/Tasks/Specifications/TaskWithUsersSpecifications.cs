using Domain.Common;

namespace Application.Features.Tasks.Specifications;

public class TaskWithAssignedToUsersSpecifications : Specification<Domain.Entities.Task.Task>
{
    public TaskWithAssignedToUsersSpecifications(Guid taskId)
    {
        Criteria= t=>t.Id==taskId;
        AddInclude(t => t.UserAssigments);
    }
}
