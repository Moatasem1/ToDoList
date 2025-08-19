using Application.Features.Users.Contracts;

namespace Application.Features.Tasks.Contracts;

public record TaskFullDetailsDto(string Name, string? Description, DateTime StartDate,DateTime EndDate,List<UserBasicDetailsDto>AssignedTo);


