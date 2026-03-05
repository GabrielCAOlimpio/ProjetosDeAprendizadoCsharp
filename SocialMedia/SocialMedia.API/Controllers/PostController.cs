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
[Route("api/v1/posts")]
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
        try
        {
            var posts = await _postService.GetAllPostsAsync(pageNumber, pageSize);
            return Ok(posts);
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        try
        {
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateInputDTO inputDTO)
    {
        Console.WriteLine("---- Entrou no CreatePost ----");

        try
        {
            var newPost = new PostsRequestDTO
            {
                Content = inputDTO.Content,
                UserId = GetCurrentUserId()
            };
            await _postService.CreatePostAsync(newPost);
            return Created();
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] PostCreateInputDTO post)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var currentUserId = GetCurrentUserId();


            var existingPost = await _postService.GetPostByIdAsync(id);
            

            dynamic postVerification = existingPost; 
            if (postVerification.UserId != currentUserId)
                throw new UnauthorizedAccessException("You do not have permission to edit this post.");

            var newPost = new PostsRequestDTO()
            {
                Content = post.Content,
                UserId = currentUserId
            };
            
            await _postService.UpdatePostAsync(id, newPost);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            var existingPost = await _postService.GetPostByIdAsync(id);
            
            dynamic postVerification = existingPost;
            if (postVerification.UserId != currentUserId)
                throw new UnauthorizedAccessException("You do not have permission to delete this post.");

            await _postService.DeletePostAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikePost([FromRoute] int id)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            await _postService.LikePostAsync(id, currentUserId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpDelete("{id}/like")]
    public async Task<IActionResult> UnlikePost([FromRoute] int id) 
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            await _postService.UnlikePostAsync(id, currentUserId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }
    

    [Authorize]
    [HttpPost("comments")]
    public async Task<IActionResult> AddComment([FromBody] CommentRequestDTO comment)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {

            comment.UserId = GetCurrentUserId(); 
            
            await _commentService.AddCommentAsync(comment);
            return Created();
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Forbid(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    } 
}