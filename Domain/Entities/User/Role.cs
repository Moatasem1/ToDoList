using Domain.Common;
using Domain.Common.Interfaces;

namespace Domain.Entities.User;

public class Role : Enumeration,IEnitity
{
    protected Role(int id, string name) : base(id, name)
    {
    }

    public static readonly Role Admin = new(1,"Admin");
}