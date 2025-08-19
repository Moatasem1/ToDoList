using System;

namespace Application.Features.Tasks.Contracts.Requests;

public record GetAvailableUsersForTask(string SearchTerm, int PageSize, List<Guid> ExcludedUserIds);
