

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Interfaces.Comments;
using Asp.Versioning;
using SocialMedia.Domain.Interfaces.Posts;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    private int GetCurrentUserId()
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
                return userId;
                
            throw new UnauthorizedAccessException("User not authenticated or invalid token.");
        }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        var userId = GetCurrentUserId();
        await _commentService.DeleteCommentAsync(id, userId); 
        
        return NoContent();
    }
}
