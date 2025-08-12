using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts.Requests;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Users.Commands;

public class DeleteUserCommand(IRepository<User> userRepo,IReadOnlyRepository<User> userReadOnlyRepo) : IUseCase
{
    public async Task<Result<bool, Error>> Handle(Guid userId)
    {
        var canHandel = await CanHandle(userId);
        if (canHandel.IsFailure) return canHandel.Error;

        var user = await userRepo.Get(new UserSpecfications(userId));

       if(user == null) return Error.NotFound(nameof(DeleteUserCommand),nameof(User.Id), userId.ToString());

       userRepo.Remove(user);

        return true;
    }

    private async Task<Result<bool,Error>> CanHandle(Guid userId)
    {
        return true;
    }
}