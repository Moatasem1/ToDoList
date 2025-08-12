using Domain.Common;
using Domain.Entities.User;

namespace Application.Features.Users.Specifications;

public class UserSpecfications : Specification<User>
{
    public UserSpecfications(Guid id)
    {
        Criteria = u => u.Id == id;
    }

    public UserSpecfications(List<Guid> ids)
    {
        Criteria = u => ids.Contains(u.Id);
    }

    public UserSpecfications(string email , Guid? exclude=null)
    {
        Criteria = u => u.Email == email && (exclude.HasValue ? u.Id != exclude : true);
    }
}
