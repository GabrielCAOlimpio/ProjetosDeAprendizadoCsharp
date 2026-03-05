namespace SocialMedia.Domain.Interfaces.Auth;


public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string passwordHash, string inputPassword);
}