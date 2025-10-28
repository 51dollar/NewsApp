using Microsoft.AspNetCore.Identity;
using WebNews.Models.Entities;

namespace WebNews.Helpers.Auth;

public class Hasher
{
    private readonly PasswordHasher<User> _passwordHasher;

    public Hasher()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string passwordHash, string passwordRequest)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, passwordHash, passwordRequest);
        return result == PasswordVerificationResult.Success;
    }
}