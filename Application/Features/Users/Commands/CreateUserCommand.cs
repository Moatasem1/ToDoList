using Application.Features.Users.Contracts.Requests;
using Application.Features.Users.Specifications;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;
using PasswordGenerator;

namespace Application.Features.Users.Commands;

public class CreateUserCommand(IEmailService emailService, IPasswordHasher passwordHasher,IBase64ByteConverter base64ByteConverter,IRepository<User> userRepo,IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
    public async Task<Result<bool, Error>> Handle(CreateUserRequest request)
    {   
        var canHandel = await CanHandle(request);
        if (canHandel.IsFailure) return canHandel.Error;

        var imageToAdd = !string.IsNullOrEmpty(request.Image) ? base64ByteConverter.Base64ToBytes(request.Image) : null;

        var creationResult = User.Create(request.FirstName, request.LastName,request.Email, imageToAdd);
        if (creationResult.IsFailure) return creationResult.Error;

        var user = creationResult.Value;

        var generatedPasword = GeneratePassword();
        var hashedPassword = passwordHasher.HashPassword(generatedPasword, user);

        var updationResult= user.UpdatePassword(hashedPassword);
       if (updationResult.IsFailure) return updationResult.Error;

        if (request.IsAdmin) AddAdminRole(user);

        await userRepo.Add(creationResult.Value);

        await emailService.SendAccountCreatedEmail(user.Email, user.FullName, generatedPasword);

        return true;
    }

    private async Task<Result<bool,Error>> CanHandle(CreateUserRequest request)
    {
       var userCount = await userReadOnlyRepo.Count(new UserSpecfications(request.Email));

        if(userCount==1) return Error.ValueAlreadyExists(nameof(CreateUserCommand),nameof(User.Email),request.Email);

        return true;
    }

    private static string GeneratePassword()
    {
        var pwd = new Password();
       return pwd.Next();
    }

    private static Result<bool,Error> AddAdminRole(User user)
    {
        var roleCreation = UserRole.Create(user.Id, Role.Admin.Id);
        if (roleCreation.IsFailure) return roleCreation.Error;

        var roleAdditionResult = user.AddRole(roleCreation.Value);
        if(roleAdditionResult.IsFailure) return roleAdditionResult.Error;

        return true;
    }
}