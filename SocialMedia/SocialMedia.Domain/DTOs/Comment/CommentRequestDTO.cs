using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Domain.DTOs.Comments;


public class CommentRequestDTO
{
    [MaxLength(300, ErrorMessage = "Content cannot exceed 300 characters.")]
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int PostId { get; set; }
}