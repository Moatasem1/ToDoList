using Application.Features.Auth.Contracts;
using Application.Features.Auth.Contracts.Requests;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Auth.Commands;

public class LoginCommand(IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
    public async Task<Result<LoginResponse, Error>> Handle(LoginRequest request)
    {
        //hashpassword before compare
        var user = await userReadOnlyRepo.Get(new UserSpecfications(request.Email,request.Password));

        if (user == null)
            return Error.NotFound(nameof(User), $"{nameof(User.Email)} {nameof(User.Password)}", $"{request.Email} {request.Password}");


        return new LoginResponse(
                user.Id,
                user.Email
            );
    }
}