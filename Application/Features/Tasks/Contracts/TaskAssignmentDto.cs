using Application.Features.Users.Contracts;

namespace Application.Features.Tasks.Contracts;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="StatusId">null in case it return task created by user</param>
/// <param name="StatusName">null in case it return task created by user</param>
/// <param name="AssignedTo"></param>
/// <param name="AssignedBy"></param>
public record TaskAssignmentDto(Guid Id, string Name,string? Description,DateTime StartDate, DateTime EndDate, int? StatusId, string? StatusName, List<UserBasicDetailsDto> AssignedTo, UserBasicDetailsDto AssignedBy);