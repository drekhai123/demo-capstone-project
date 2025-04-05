﻿using EduSource.Contract.Abstractions.Services;

namespace EduSource.Infrastructure.Services;

public class PasswordHashService : IPasswordHashService
{
    private readonly int workFactor = 13;
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor);
    }

    public bool VerifyPassword(string password, string passwordHashed)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHashed);
    }
}
