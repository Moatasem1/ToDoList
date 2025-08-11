using Application.Features.Users.Contracts;

namespace Application.Features.Tasks.Contracts;

public record TaskAssignmentDto(Guid Id, string Name, List<UserBasicDetailsDto> AssignedTo, Guid AssignedBy);