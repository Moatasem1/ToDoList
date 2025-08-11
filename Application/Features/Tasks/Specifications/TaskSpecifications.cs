using Domain.Common;

namespace Application.Features.Tasks.Specifications;

public class TaskSpecifications : Specification<Domain.Entities.Task.Task>
{
    public TaskSpecifications(Guid taskId)
    {
        Criteria= t=>t.Id==taskId;
    }
}
