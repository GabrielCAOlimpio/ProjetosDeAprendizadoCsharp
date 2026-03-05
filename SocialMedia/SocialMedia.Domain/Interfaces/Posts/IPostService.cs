using SocialMedia.Domain.DTOs.Comments;
using SocialMedia.Domain.DTOs.Posts;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces.Posts;

public interface IPostService
{
    Task<List<PostItemDTO>> GetAllPostsAsync(int pages = 1, int pageSize = 10);
    Task<PostsResponseDTO> GetPostByIdAsync(int postId);
    Task CreatePostAsync(PostsRequestDTO post);
    Task UpdatePostAsync(int postId, PostsRequestDTO post);
    Task DeletePostAsync(int postId);

    // Additional methods for likes
    Task LikePostAsync(int postId, int userId);
    Task UnlikePostAsync(int postId, int userId);
 
}