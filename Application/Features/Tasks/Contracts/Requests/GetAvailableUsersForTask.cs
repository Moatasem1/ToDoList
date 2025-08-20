using FluentValidation;
using System;

namespace Application.Features.Tasks.Contracts.Requests;

public record GetAvailableUsersForTask(string SearchTerm, int PageSize, List<Guid> ExcludedUserIds);


public class GetAvailableUsersForTaskValidator : AbstractValidator<GetAvailableUsersForTask> { 

    public GetAvailableUsersForTaskValidator() {
        RuleFor(r => r.PageSize)
            .GreaterThan(0);
    }
}