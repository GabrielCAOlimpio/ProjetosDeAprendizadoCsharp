namespace SocialMedia.Domain.Interfaces.Auth;

using SocialMedia.Domain.DTOs.Auth;
using SocialMedia.Domain.DTOs.Users;
public interface IAuthService
{
    Task<TokenResponseDTO> Login(LoginRequestDTO dto);
    Task<UserResponseDTO> Register(RegisterRequestDTO dto);
}