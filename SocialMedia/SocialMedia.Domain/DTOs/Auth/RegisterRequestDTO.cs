using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Domain.DTOs.Auth;

public class RegisterRequestDTO
{
    [Required]
    [MinLength(4)]
    public string Username {get;set;} = string.Empty;


    [Required]
    [EmailAddress]
    public string Email {get;set;} = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password {get;set;} = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string RepeatPassword {get;set;} = string.Empty;

    
}