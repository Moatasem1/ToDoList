using Domain.Entities.User;

namespace Application.Services.Interfaces;

public interface IPasswordHasher{
    string HashPassword(string password,User user);
    bool VerifyPassword(string password, string hashedPassword, User user);
}
