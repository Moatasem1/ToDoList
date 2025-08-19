using System;
using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Users.Contracts;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Tasks.Queries;

public class GetAvailableUsersForTaskQuery(IReadOnlyRepository<User> userReadOnlyRepo, IBase64ByteConverter base64ByteConverter) : IUseCase
{
    public async Task<Result<List<UserBasicDetailsDto>, Error>> Handle(GetAvailableUsersForTask request)
    {
        var paginatedUsers = await userReadOnlyRepo.GetAll(new UserSpecfications(request.SearchTerm, request.PageSize, request.ExcludedUserIds));

        var data = paginatedUsers.Select(u =>
        {
            var imageBase64 = u.Image == null ? null : base64ByteConverter.BytesToBase64(u.Image);
            return new UserBasicDetailsDto(u.Id, u.FullName, u.Email, imageBase64);
        }).ToList();

        return data;
    }
}
