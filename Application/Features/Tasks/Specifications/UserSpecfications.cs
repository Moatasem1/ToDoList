using Domain.Common;
using Domain.Entities.User;

namespace Application.Features.Tasks.Specifications;

public class UserSpecfications : Specification<User>
{
    public UserSpecfications(Guid id)
    {
        Criteria = u => u.Id == id;
    }
}
