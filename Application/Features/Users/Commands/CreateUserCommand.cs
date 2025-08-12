using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts.Requests;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Commands;

public class CreateUserCommand(IBase64ByteConverter base64ByteConverter,IRepository<User> userRepo,IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
   
    public async Task<Result<bool, Error>> Handle(CreateUserRequest request)
    {
        
        var canHandel = await CanHandle(request);
        if (canHandel.IsFailure) return canHandel.Error;

        var imageToAdd = !string.IsNullOrEmpty(request.Image) ? base64ByteConverter.Base64ToBytes(request.Image) : null;

        var creationResult = User.Create(request.FirstName, request.LastName,request.Email, imageToAdd);
        if (creationResult.IsFailure) return creationResult.Error;

        await userRepo.Add(creationResult.Value);

        return true;
    }

    private async Task<Result<bool,Error>> CanHandle(CreateUserRequest request)
    {
       var userCount = await userReadOnlyRepo.Count(new UserSpecfications(request.Email));

        if(userCount==1) return Error.ValueAlreadyExists(nameof(CreateUserCommand),nameof(User.Email),request.Email);

        return true;
    }
}