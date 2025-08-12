using Domain.Common;

namespace Application.Features.Tasks.Specifications;

public class TaskAssignmentAssignedToUserSpecification : Specification<Domain.Entities.User.UserTaskAssignment>
{
    public TaskAssignmentAssignedToUserSpecification(Guid userId) { 
            Criteria = t=>t.UserId == userId;
    }
}
