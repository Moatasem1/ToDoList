using Domain.Common;

namespace Application.Features.Tasks.Specifications;

public class TaskWithAssignedUsersSpecifications : Specification<Domain.Entities.Task.Task>
{
    public TaskWithAssignedUsersSpecifications(Guid taskId)
    {
        Criteria= t=>t.Id==taskId;
        AddInclude(t => t.UserAssigments);
    }
    public TaskWithAssignedUsersSpecifications(List<Guid> taskIds)
    {
        Criteria = t => taskIds.Contains(t.Id);
        AddInclude(t => t.UserAssigments);
    }
}
