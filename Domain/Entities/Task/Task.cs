using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Domain.Entities.Task;

public class Task : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public string? Description { get; private set; }

    public  DateTime StartAt { get; private set; }

    public  DateTime EndAt { get; private set; }

    public Guid CreatedBy { get; private set; }

    public IReadOnlyCollection<UserTaskAssignment> UserAssigments => _userAssigments.AsReadOnly();
    private readonly List<UserTaskAssignment> _userAssigments = [];

    private Task() { }

    public static Result<Task, Error> Create(string name,string? description, Guid createdBy, DateTime startDate, DateTime endDate)
    {
        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure)
            return nameValidation.Error;

        var descriptionValidation = ValidateDescription(description);
        if (descriptionValidation.IsFailure)
            return descriptionValidation.Error;

        var startDateValidation = ValidateStartDate(startDate);
        if (startDateValidation.IsFailure)
            return startDateValidation.Error;

        var endDateValidation = ValidateEndDate(endDate, startDate);
        if (endDateValidation.IsFailure)
            return endDateValidation.Error;

        var task = new Task
        {
            Id = Guid.NewGuid(),
            Name = name.Trim().ToLowerInvariant(),
            CreatedBy = createdBy,
            StartAt = startDate,
            EndAt = endDate,
            Description = description?.ToLowerInvariant().Trim()
        };

        return task;
    }

    public Result<bool,Error> Update(string name,string? description, DateTime startDate,DateTime endDate)
    {
        var nameValidation = ValidateName(name);
        if (nameValidation.IsFailure)
            return nameValidation.Error;

        var descriptionValidation = ValidateDescription(description);
        if (descriptionValidation.IsFailure)
            return descriptionValidation.Error;

        var startDateValidation = ValidateStartDate(startDate);
        if (startDateValidation.IsFailure)
            return startDateValidation.Error;

        var endDateValidation = ValidateEndDate(endDate,startDate);
        if (endDateValidation.IsFailure)
            return endDateValidation.Error;

        Name = name.Trim().ToLowerInvariant();
        Description = description?.ToLowerInvariant().Trim();
        StartAt = startDate;
        EndAt = endDate;
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

    public static Result<bool, Error> ValidateDescription(string? description)
    {
        description = description?.Trim();
        if (string.IsNullOrEmpty(description))
            return true;

        if (description.Length < 3 || description.Length > 1500)
            return Error.ValueInvalid(nameof(Task), nameof(Description));

        return true;
    }

    public static Result<bool, Error> ValidateStartDate(DateTime startDate)
    {
        if (startDate <= DateTime.Now)
            return Error.ValueInvalidWithMessage(nameof(Task),$"{nameof(startDate)} must be a future date and time");

        return true;
    }

    public static Result<bool, Error> ValidateEndDate(DateTime endDate,DateTime startDate)
    {
        if (endDate <= startDate)
            return Error.ValueInvalidWithMessage(nameof(Task), $"{nameof(endDate)} must be later than {nameof(startDate)}");

        return true;
    }
}