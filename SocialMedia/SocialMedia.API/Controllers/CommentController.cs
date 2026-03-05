

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Interfaces.Comments;
using Asp.Versioning;
using SocialMedia.Domain.Interfaces.Posts;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/comments")]

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
        try
        {
            var userId = GetCurrentUserId();
            await _commentService.DeleteCommentAsync(id,userId); 
            
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (InvalidOperationException ex) // Caso o serviço lance isso se o User não for o dono
        {
            return Forbid(ex.Message);
        }
        catch (Exception)
        {
            return Problem("An error occurred, try again later.");
        }
    }
}
