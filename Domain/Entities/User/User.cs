using Domain.Common;
using Domain.Common.Interfaces;

namespace Domain.Entities.User;

public class User : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public byte[]? Image { get; private set; }

    private User() { }

    public string FullName => $"{FirstName} {LastName}";

    public static Result<User,Error>Create(string firstName,string lastName,string email, byte[]? image)
    {
        var firstNameValidation = ValidateFirstName(firstName);
        if (firstNameValidation.IsFailure)
            return firstNameValidation.Error;

        var lastNameValidation = ValidateLastName(lastName);
        if (lastNameValidation.IsFailure)
            return lastNameValidation.Error;

        var emailValidation = ValidateEmail(email);
        if (emailValidation.IsFailure)
            return emailValidation.Error;


        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim().ToLowerInvariant(),
            LastName = lastName.Trim().ToLowerInvariant(),
            Email = email.Trim(),
            Image = image
        };

        return user;
    }

    public Result<bool,Error> Update(string firstName, string lastName, string email, byte[]? image)
    {
        var firstNameValidation = ValidateFirstName(firstName);
        if (firstNameValidation.IsFailure)
            return firstNameValidation.Error;

        var lastNameValidation = ValidateLastName(lastName);
        if (lastNameValidation.IsFailure)
            return lastNameValidation.Error;

        var emailValidation = ValidateEmail(email);
        if (emailValidation.IsFailure)
            return emailValidation.Error;

        FirstName = firstName.Trim().ToLowerInvariant();
        LastName = lastName.Trim().ToLowerInvariant();
        Email = email.Trim();
        Image = image;

        return true;
    }

    //validators
    public static Result<bool,Error> ValidateFirstName(string firstName)
    {
        firstName = firstName.Trim();
        if (string.IsNullOrEmpty(firstName))
                return Error.ValueRequired(nameof(User), nameof(FirstName));

        if (firstName.Length < 3 || firstName.Length > 20)
                return Error.ValueInvalid(nameof(User), nameof(FirstName));

        return true;
    }

    public static Result<bool, Error> ValidateLastName(string lastName)
    {
        lastName = lastName.Trim();
        if (string.IsNullOrEmpty(lastName))
            return Error.ValueRequired(nameof(User), nameof(LastName));

        if (lastName.Length < 3 || lastName.Length > 20)
            return Error.ValueInvalid(nameof(User), nameof(LastName));

        return true;
    }

    public static Result<bool, Error> ValidateEmail(string email)
    {
        email = email.Trim();
        if (string.IsNullOrEmpty(email))
            return Error.ValueRequired(nameof(User), nameof(Email));

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                return Error.ValueInvalid(nameof(User), nameof(Email));
        }
        catch
        {
            return Error.ValueInvalid(nameof(User), nameof(Email));
        }

        return true;
    }
}
