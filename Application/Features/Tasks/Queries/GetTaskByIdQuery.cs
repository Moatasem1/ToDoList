using Application.Features.Tasks.Contracts;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;
using System.Linq;

namespace Application.Features.Tasks.Queries;

public sealed class GetTaskByIdQuery(IReadOnlyRepository<User> userReadOnlyRepo,IReadOnlyRepository<Domain.Entities.Task.Task> taskReadOnlyRepo, IBase64ByteConverter base64ByteConverter) : IUseCase
{
    public async Task<Result<TaskFullDetailsDto, Error>> Handle(Guid taskId,Guid userId)
    {

        var task = await taskReadOnlyRepo.Get(new TaskWithAssignedUsersSpecifications(taskId));

        if (task == null || task.CreatedBy!=userId) return Error.NotFound(nameof(GetTaskByIdQuery),nameof(Task.Id),taskId.ToString());

        var userIds = task.UserAssigments
            .Select(us=>us.UserId)
            .ToHashSet();

        var assignedUsers = await userReadOnlyRepo.GetAll(new UserSpecfications([..userIds]));

        return new TaskFullDetailsDto(
            task.Name,
            task.Description,
            task.StartAt,
            task.EndAt,
            assignedUsers.Select(u =>
            {
                var imge = u.Image is null ? null : base64ByteConverter.BytesToBase64(u.Image);
                return new UserBasicDetailsDto(u.Id, u.FullName, u.Email, imge);
            }).ToList()
        );
    }
}
