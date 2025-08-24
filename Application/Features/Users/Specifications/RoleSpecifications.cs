using Domain.Common;
using Domain.Entities.User;

namespace Application.Features.Users.Specifications;

public class RoleSpecifications : Specification<Role>
{
    public RoleSpecifications(List<int>ids) { 
        Criteria = r=>ids.Contains(r.Id);
    }
}
