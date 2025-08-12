using Application.Features.Users.Contracts;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Queries;

public class GetAllUsersQuery(IReadOnlyRepository<User> userReadOnlyRepo,IBase64ByteConverter base64ByteConverter) : IUseCase
{
    public async Task<Result<List<UserBasicDetailsDto>, Error>> Handle()
    {
        var users = await userReadOnlyRepo.GetAll();

        var result = users.Select(u =>
        {
            var imageBase64 = (u.Image == null ? null : base64ByteConverter.BytesToBase64(u.Image));
            return new UserBasicDetailsDto(u.Id, u.FullName, imageBase64);
        }).ToList();

        return result;
    }
}