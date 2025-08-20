using Application.Features.Users.Contracts;
using Application.Features.Users.Specifications;
using Application.Options;
using Application.Services.Interfaces;
using DataTables.AspNet.Core;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Queries;

public class GetAllUsersQuery(IReadOnlyRepository<User> userReadOnlyRepo,IBase64ByteConverter base64ByteConverter) : IUseCase
{
    public async Task<Result<MyDataTableResponse<UserBasicDetailsDto>, Error>> Handle(IDataTablesRequest request)
    {
        var usersCount = await userReadOnlyRepo.Count();

        var filteredUsersCount = await userReadOnlyRepo.Count(new UserSpecfications(request));

        var paginatedUsers = await userReadOnlyRepo.GetAll(new UserSpecfications(request, true));

        var data = paginatedUsers.Select(u =>
        {
            var imageBase64 = (u.Image == null ? null : base64ByteConverter.BytesToBase64(u.Image));
            return new UserBasicDetailsDto(u.Id, u.FullName, u.Email, imageBase64);
        }).ToList();

        var response =new MyDataTableResponse<UserBasicDetailsDto>(
            request.Draw,
            usersCount,
            filteredUsersCount,
            data
            );

        return response;
    }
}