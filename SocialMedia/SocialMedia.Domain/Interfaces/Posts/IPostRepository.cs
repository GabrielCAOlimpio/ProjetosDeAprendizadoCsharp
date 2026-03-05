namespace SocialMedia.Domain.Interfaces.Posts;

using SocialMedia.Domain.Entities;

public interface IPostRepository
{
    Task<List<Post>> GetAllAsync(int pages = 1, int pageSize = 10);
    Task<Post> GetByIdAsync(int id);
    Task CreateAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int id);

    // Additional methods for likes
    Task LikePostAsync(int postId, int userId);
    Task UnlikePostAsync(int postId, int userId);

    // Additional methods for comments
    
    
}