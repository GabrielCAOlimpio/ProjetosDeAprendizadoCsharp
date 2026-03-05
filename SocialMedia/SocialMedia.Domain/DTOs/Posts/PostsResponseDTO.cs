using SocialMedia.Domain.DTOs.Comments;
using SocialMedia.Domain.DTOs.Users;

namespace SocialMedia.Domain.DTOs.Posts;

public class PostItemDTO
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public List<CommentResponseDTO> Comments { get; set; } = [];
}

public class PostsResponseDTO
{
    public UserResponseDTO User { get; set; } = null!;
    public List<PostItemDTO> Posts { get; set; } = [];
}