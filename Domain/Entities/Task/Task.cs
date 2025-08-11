using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Domain.Entities.Task;

public class Task : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public IReadOnlyCollection<UserTaskAssignment> UserAssigments => _userAssigments.AsReadOnly();
    private readonly List<UserTaskAssignment> _userAssigments = [];

    private Task() { }

    public static Result<Task, Error> Create(string name)
    {
        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure)
            return nameValidation.Error;

        var task = new Task
        {
            Id = Guid.NewGuid(),
            Name = name.Trim().ToLowerInvariant()
        };

        return task;
    }

    public Result<bool,Error> Update(string name)
    {
        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure)
            return nameValidation.Error;

        Name = name.Trim().ToLowerInvariant();
        return true;
    }

    public Result<bool, Error> AddUserAssignment(UserTaskAssignment userTaskAssigment)
    {
        _userAssigments.Add(userTaskAssigment);
        return true;
    }
    public Result<bool, Error> RemoveUserAssignment(Guid userId)
    {
        var assigmentToRemove = _userAssigments.Find(t => t.UserId == userId);

        if (assigmentToRemove == null)
            return Error.NotFound(nameof(User.User), nameof(User.User.Id), userId.ToString());

        _userAssigments.Remove(assigmentToRemove);

        return true;
    }

    public Result<bool,Error> ClearAll()
    {
        _userAssigments.Clear();
        return true;
    }


    //validators
    public static Result<bool, Error> ValidateName(string name)
    {
        name = name.Trim();
        if (string.IsNullOrEmpty(name))
            return Error.ValueRequired(nameof(Task), nameof(Name));

        if (name.Length < 3 || name.Length > 150)
            return Error.ValueInvalid(nameof(Task), nameof(Name));

        return true;
    }
}