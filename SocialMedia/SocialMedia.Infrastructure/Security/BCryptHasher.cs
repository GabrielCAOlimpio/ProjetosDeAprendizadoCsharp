using SocialMedia.Domain.Interfaces.Auth;
using SocialMedia.Infrastructure.Security.Auth;

namespace SocialMedia.Infrastructure.Security;

public class BCryptHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool Verify(string hash, string input) => BCrypt.Net.BCrypt.Verify(input, hash);
}