using Application.Services.Interfaces;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class PasswordHasher(PasswordHasher<User> passwordHasher) : IPasswordHasher
{
    public string HashPassword(string password,User user)
    {
        return passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(string password, string hashedPassword,User user)
    {
        return passwordHasher.VerifyHashedPassword(user, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}
