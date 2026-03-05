namespace SocialMedia.Domain.DTOs.Users;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserRequestDTO
{

    [Required]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "Bio cannot exceed 200 characters.")]
    public string? Bio {get;set;}

}