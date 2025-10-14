using Microsoft.AspNetCore.Identity;

namespace WebNews.Helpers;

public class Hasher<T> where T : class
{
    private readonly PasswordHasher<T> _passwordHasher;

    public Hasher()
    {
        _passwordHasher = new PasswordHasher<T>();
    }

    public string HashPassword(T user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(T user, string passwordHash, string passwordRequest)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, passwordHash, passwordRequest);
        return result == PasswordVerificationResult.Success;
    }
}