using Domain.Common;

namespace Application.Features.Tasks.Specifications;

public class TaskSpecifications : Specification<Domain.Entities.Task.Task>
{
    public TaskSpecifications(Guid taskId)
    {
        Criteria= t=>t.Id==taskId;
    }

    public TaskSpecifications(string name)
    {
        Criteria = t => t.Name.ToLower().Trim() == name.ToLower().Trim();
    }

    public TaskSpecifications(List<Guid> taskIds)
    {
        Criteria = t => taskIds.Contains(t.Id);
    }
}
