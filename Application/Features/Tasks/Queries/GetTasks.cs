using Application.Features.Tasks.Contracts;
using Application.Features.Tasks.Contracts.Requests;
using Application.Features.Tasks.Specifications;
using Application.Features.Users.Contracts;
using Application.Features.Users.Specifications;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.User;

namespace Application.Features.Tasks.Queries;

public sealed class GetTasks(GetUserTasksQuery getUserTasksQuery,GetTasksCreatedByUserQuery getTasksCreatedByUserQuery) : IUseCase
{
    public async Task<Result<List<TaskAssignmentDto>, Error>> Handle(Guid userId, GetTaskFilter taskFilter)
    {
        var result = taskFilter switch
        {
            GetTaskFilter.Created => await getUserTasksQuery.Handle(userId),
            GetTaskFilter.Assigned => await getTasksCreatedByUserQuery.Handle(userId),
            _ => Error.ValueInvalid(nameof(GetTasks), nameof(taskFilter))
        };

        return result;
    }
}
