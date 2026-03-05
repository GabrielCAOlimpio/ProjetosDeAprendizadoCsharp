namespace SocialMedia.Domain.DTOs.Users;

public class UserResponseDTO
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Bio { get; set; }
}