namespace SocialMedia.Domain.DTOs.Comments;

public class CommentResponseDTO
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}