namespace SocialMedia.Domain.Entities;

public class Post 
{
    public int PostId { get; set; } 
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int LikesCount { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Like> Likes { get; set; } = null!;

    public List<Comment> Comments { get; set; } = [];

}