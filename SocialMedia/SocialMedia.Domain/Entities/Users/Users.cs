namespace SocialMedia.Domain.Entities;

/*
- Id
- Username (único)
- Email (único)
- Bio (opcional)
- CreatedAt
- IsActive
*/

public class User
{
    public int UserId {get;set;}
    public string Username {get;set;} = string.Empty;
    public string Email {get;set;} = string.Empty;
    public string PasswordHash {get;set;} = string.Empty;
    public string? Bio {get;set;}
    public DateTime CreatedAt {get;set;}
    public bool IsActive { get; set; } = true;

    public List<Post> Posts {get;set;} = null!;
    public List<Like> Likes { get; set; } = null!;

    public List<Comment> Comments { get; set; } = null!;
}