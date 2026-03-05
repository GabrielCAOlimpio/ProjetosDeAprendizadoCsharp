namespace SocialMedia.Domain.DTOs.Posts;

using System.ComponentModel.DataAnnotations;

public class PostCreateInputDTO
{
    [Required]
    [MinLength(1, ErrorMessage = "Content must be at least 1 character long.")]
    [MaxLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
    public string Content { get; set; } = string.Empty;
}