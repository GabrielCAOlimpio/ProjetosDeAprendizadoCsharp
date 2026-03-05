namespace SocialMedia.Domain.Services;

using SocialMedia.Domain.Entities;
using SocialMedia.Domain.DTOs.Comments;
using SocialMedia.Domain.Interfaces.Comments;
using SocialMedia.Domain.Interfaces.Posts;
using SocialMedia.Domain.Interfaces.Users;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public CommentService(ICommentRepository commentRepository, IPostService postService, IUserService userService)
    {
        _commentRepository = commentRepository;
        _postService = postService;
        _userService = userService;
    }

    public async Task AddCommentAsync(CommentRequestDTO commentDto)
    {
        if (commentDto == null || string.IsNullOrWhiteSpace(commentDto.Content))
        {
            throw new ArgumentException("Comment content cannot be empty.");
        }
        if (commentDto.UserId <= 0 || commentDto.PostId <= 0)
        {
            throw new ArgumentException("User ID and Post ID must be positive numbers.");
        }
        
        var post = await _postService.GetPostByIdAsync(commentDto.PostId);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {commentDto.PostId} not found.");
        }

        var user = await _userService.GetUserByIdAsync(commentDto.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {commentDto.UserId} not found.");
        }


        
        var comment = new Comment
        {
            Content = commentDto.Content,
            UserId = commentDto.UserId,
            PostId = commentDto.PostId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddCommentAsync(comment);
    }

    public async Task DeleteCommentAsync(int commentId, int userId)
    {
        if (commentId <= 0)
            throw new ArgumentException("Comment ID must be a positive number.");

        if (userId <= 0)
            throw new ArgumentException("User Id must be a positive number.");


        await _commentRepository.DeleteCommentAsync(commentId,userId);
    }
}