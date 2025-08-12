using Application.Features.Users.Contracts;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Queries;

public class GetUserQuery(IReadOnlyRepository<User> userReadOnlyRepo,IBase64ByteConverter base64ByteConverter) : IUseCase
{
    public async Task<Result<UserDto, Error>> Handle(Guid userId)
    {
        var user = await userReadOnlyRepo.Get(new UserSpecfications(userId));

        if (user == null) return Error.NotFound(nameof(GetUserQuery),nameof(User.Id),userId.ToString());

        var imageBase64 = (user.Image == null) ? null : base64ByteConverter.BytesToBase64(user.Image);
        return new UserDto(userId, user.FirstName, user.LastName, user.Email, imageBase64);
    }
}