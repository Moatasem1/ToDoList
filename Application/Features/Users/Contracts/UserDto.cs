namespace Application.Features.Users.Contracts;

public record UserDto(Guid Id, string FirstName,string LastName,string Email, string? Image);
