using Domain.Common;

namespace Application.Features.Tasks.Specifications;

public class TaskCreatedBySpecifications : Specification<Domain.Entities.Task.Task>
{
    public TaskCreatedBySpecifications(Guid userId)
    {
        Criteria= t=>t.CreatedBy==userId;
        AddInclude(t => t.UserAssigments);
    }
   
}
