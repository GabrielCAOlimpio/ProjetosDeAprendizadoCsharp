using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Interfaces.Posts;
using SocialMedia.Domain.DTOs.Posts;
using SocialMedia.Domain.DTOs.Comments;
using SocialMedia.Domain.Interfaces.Comments;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public PostController(IPostService postService, ICommentService commentService)
    {
        _postService = postService;
        _commentService = commentService;
    }

    private int GetCurrentUserId()
    {
        if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            return userId;
            
        throw new UnauthorizedAccessException("User not authenticated or invalid token.");
    }

    [HttpGet("{pageNumber}/{pageSize}")]
    public async Task<IActionResult> GetAllPosts([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 10)
    {
        var posts = await _postService.GetAllPostsAsync(pageNumber, pageSize);
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        return Ok(post);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateInputDTO inputDTO)
    {
        var newPost = new PostsRequestDTO
        {
            Content = inputDTO.Content,
            UserId = GetCurrentUserId()
        };
        await _postService.CreatePostAsync(newPost);
        return Created();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] PostCreateInputDTO post)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var currentUserId = GetCurrentUserId();
        
        var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var existingPost = await _postService.GetPostByIdAsync(id);

        if (existingPost.User.Email != currentUserEmail)
            throw new UnauthorizedAccessException("You do not have permission to edit this post.");

        var newPost = new PostsRequestDTO()
        {
            Content = post.Content,
            UserId = currentUserId
        };
        
        await _postService.UpdatePostAsync(id, newPost);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var existingPost = await _postService.GetPostByIdAsync(id);
        
        if (existingPost.User.Email != currentUserEmail)
            throw new UnauthorizedAccessException("You do not have permission to delete this post.");

        await _postService.DeletePostAsync(id);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikePost([FromRoute] int id)
    {
        var currentUserId = GetCurrentUserId();
        await _postService.LikePostAsync(id, currentUserId);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}/like")]
    public async Task<IActionResult> UnlikePost([FromRoute] int id) 
    {
        var currentUserId = GetCurrentUserId();
        await _postService.UnlikePostAsync(id, currentUserId);
        return NoContent();
    }
    

    [Authorize]
    [HttpPost("comments")]
    public async Task<IActionResult> AddComment([FromBody] CommentRequestDTO comment)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        comment.UserId = GetCurrentUserId(); 
        
        await _commentService.AddCommentAsync(comment);
        return Created();
    } 
}