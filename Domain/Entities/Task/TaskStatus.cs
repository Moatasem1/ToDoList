using Domain.Common;
using Domain.Common.Interfaces;

namespace Domain.Entities.Task;

public class TaskStatus : Enumeration,IEnitity
{
    protected TaskStatus(int id, string name) : base(id, name)
    {

    }

    public static readonly TaskStatus NotStarted = new (1,"not started");
    public static readonly TaskStatus InProgress = new (2,"in progress");
    public static readonly TaskStatus Completed = new (3,"completed");
}
