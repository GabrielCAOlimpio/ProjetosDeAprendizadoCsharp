namespace SocialMedia.Domain.Services;

using SocialMedia.Domain.Interfaces.Posts;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.DTOs.Posts;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Domain.DTOs.Comments;


public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;

    public PostService(IPostRepository postRepository, IUserService userService)
    {
        _postRepository = postRepository;
        _userService = userService;
    }

    public async Task<List<PostItemDTO>> GetAllPostsAsync(int pages = 1, int pageSize = 10)
    {
        if (pages <= 0 || pageSize <= 0)
            throw new ArgumentException("Page number and page size must be positive integers.");
        if (pageSize > 100)
            throw new ArgumentException("Page size cannot exceed 100.");
            
        List<Post> posts = await _postRepository.GetAllAsync(pages, pageSize);

        if (posts == null || !posts.Any())
            return new List<PostItemDTO>();

        var firstPost = posts.FirstOrDefault();
        var userThatPosted = await _userService.GetUserByIdAsync(firstPost?.UserId ?? 0);
        
        if (userThatPosted == null)
            throw new KeyNotFoundException("Author not found for the requested posts.");

        var postItems = posts.Select(p => new PostItemDTO
        {
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            LikesCount = p.LikesCount,
            Comments = p.Comments.Select(c => new CommentResponseDTO
            {
                Content = c.Content,
                CreatedAt = c.CreatedAt,
            }).ToList()
        }).ToList();

        return postItems;
        
    }



    public async Task<PostsResponseDTO> GetPostByIdAsync(int postId)
    {
        if (postId <= 0)
            throw new ArgumentException("Post ID must be a positive number.");

        Post post = await _postRepository.GetByIdAsync(postId);
        
        if (post == null)
            throw new KeyNotFoundException($"Post with ID {postId} was not found.");

        var userThatPosted = await _userService.GetUserByIdAsync(post.UserId);
        
        if (userThatPosted == null)
            throw new KeyNotFoundException("The author of this post no longer exists.");

        return new PostsResponseDTO()
        {
            User = userThatPosted,
            Posts = new List<PostItemDTO>
            {
                new PostItemDTO
                {
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    LikesCount = post.LikesCount,
                    Comments = post.Comments.Select(c => new CommentResponseDTO
                    {
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                    }).ToList()
                }
            }
        };
    }

    public async Task CreatePostAsync(PostsRequestDTO post)
    {
        if (post == null || string.IsNullOrWhiteSpace(post.Content))
            throw new ArgumentException("Post content cannot be null or empty.");

        if (post.UserId <= 0)
            throw new ArgumentException("A valid User ID is required to create a post.");

        var userExists = await _userService.GetUserByIdAsync(post.UserId);
        if (userExists == null)
            throw new KeyNotFoundException("Cannot create post: User not found.");

        var newPost = new Post
        {
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = DateTime.UtcNow,
            LikesCount = 0
        };

        await _postRepository.CreateAsync(newPost);
    }

    public async Task UpdatePostAsync(int postId, PostsRequestDTO post)
    {
        if (postId <= 0 || post.UserId <= 0)
            throw new ArgumentException("Invalid Post or User ID. Please check the data and try again.");
        
        if (post == null || string.IsNullOrWhiteSpace(post.Content))
            throw new ArgumentException("Cannot update a post with empty content.");

        var user = await _userService.GetUserByIdAsync(post.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        var existingPost = await _postRepository.GetByIdAsync(postId);

        if (existingPost == null)
            throw new KeyNotFoundException($"Post with ID {postId} not found.");

        existingPost.Content = post.Content;

        await _postRepository.UpdateAsync(existingPost);
    }

    public async Task DeletePostAsync(int postId)
    {
        if (postId <= 0)
            throw new ArgumentException("Invalid ID. Post ID must be a positive number.");
        
        var existingPost = await _postRepository.GetByIdAsync(postId);
        if (existingPost == null)
            throw new KeyNotFoundException("The post you are trying to delete does not exist.");

        await _postRepository.DeleteAsync(postId);
    }

    public async Task LikePostAsync(int postId, int userId)
    {
        if (postId <= 0 || userId <= 0)
            throw new ArgumentException("Invalid Post ID or User ID. Both must be positive numbers.");

        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new KeyNotFoundException($"Post with ID {postId} not found.");

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        await _postRepository.LikePostAsync(postId, userId);

        
    }
    public async Task UnlikePostAsync(int postId, int userId)
    {
        if (postId <= 0 || userId <= 0)
            throw new ArgumentException("Invalid Post ID or User ID. Both must be positive numbers.");

        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new KeyNotFoundException($"Post with ID {postId} not found.");

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        await _postRepository.UnlikePostAsync(postId, userId);
    }

}