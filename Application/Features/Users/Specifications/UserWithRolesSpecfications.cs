using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Specifications;

public class UserWithRolesSpecfications : Specification<User>
{
    public UserWithRolesSpecfications(string email) { 
        Criteria = u=> u.Email == email;
        AddInclude(u=>u.UserRoles);
    }
}
