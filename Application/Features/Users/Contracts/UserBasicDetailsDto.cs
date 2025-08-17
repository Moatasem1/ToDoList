namespace Application.Features.Users.Contracts;

public record UserBasicDetailsDto(Guid Id,string Name,string Email, string? Image);