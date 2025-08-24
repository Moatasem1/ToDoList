using Domain.Common;
using Domain.Common.Interfaces;

namespace Domain.Entities.User;

public class UserRole: IEnitity
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }

    public static Result<UserRole,Error> Create(Guid userId, int roleId)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}
