using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces.Auth;


public interface ITokenService
{
    string GenerateToken(User user);
}