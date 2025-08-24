using Application.Features.Auth.Contracts;
using Application.Features.Auth.Contracts.Requests;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Auth.Commands;

public class LoginCommand(IReadOnlyRepository<Role> roleReadOnlyRepo, IPasswordHasher passwordHasher, IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
    public async Task<Result<LoginResponse, Error>> Handle(LoginRequest request)
    {
        var user = await userReadOnlyRepo.Get(new UserWithRolesSpecfications(request.Email));
        
        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.Password,user))
            return Error.ValueInvalidWithMessage(nameof(User), "Email / Password are Invalid");

        var userRolesIds = user.UserRoles.Select(r=>r.RoleId).ToList();
        var userRoles = await roleReadOnlyRepo.GetAll(new RoleSpecifications(userRolesIds));

        return new LoginResponse(
                user.Id,
                user.Email,
                [.. userRoles.Select(u => u.Name)]
            );
    }
}