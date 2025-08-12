using Application.Features.Users.Contracts.Requests;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Commands;

public class UpdateUserCommand(IBase64ByteConverter base64ByteConverter,IRepository<User> userRepo,IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
    public async Task<Result<bool, Error>> Handle(Guid userId, UpdateUserRequest request)
    {
        var canHandel = await CanHandle(request,userId);
        if (canHandel.IsFailure) return canHandel.Error;

        var user = await userRepo.Get(new UserSpecfications(userId));

        var updatedImage =  (request.IsImageUpdate ? (!string.IsNullOrEmpty(request.Image) ? base64ByteConverter.Base64ToBytes(request.Image) : null) : user!.Image);

        var updateResult = user!.Update(request.FirstName, request.LastName, request.Email, updatedImage);
        if (updateResult.IsFailure) return updateResult.Error;

        userRepo.Update(user);

        return true;
    }

    private async Task<Result<bool,Error>> CanHandle(UpdateUserRequest request,Guid userId)
    {
        var userCount = await userReadOnlyRepo.Count(new UserSpecfications(userId));

        if(userCount==0) return Error.NotFound(nameof(CreateUserCommand), nameof(User.Id), userId.ToString());

        var userEmailCount = await userReadOnlyRepo.Count(new UserSpecfications(request.Email,userId));

        if(userEmailCount == 1) return Error.ValueAlreadyExists(nameof(CreateUserCommand),nameof(User.Email),request.Email);

        return true;
    }
}