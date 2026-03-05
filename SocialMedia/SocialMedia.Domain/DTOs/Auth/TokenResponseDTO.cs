using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Domain.DTOs.Auth;

public class TokenResponseDTO
{
    public string Token {get;set;} = string.Empty;
}