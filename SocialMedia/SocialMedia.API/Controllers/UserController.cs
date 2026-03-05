using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Domain.DTOs.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    private int GetCurrentUserId()
    {
        if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            return userId;
            
        throw new UnauthorizedAccessException("User not authenticated or invalid token.");
    }

    [HttpGet("{pageNumber:int?}/{pageSize:int?}")]
    public async Task<IActionResult> GetAllUsers([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 10)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be positive integers.");
            
            var users = await _userService.GetAllAsync(pageNumber, pageSize);
            return Ok(users);
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);    
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [HttpGet("{userId}/posts")]
    public async Task<IActionResult> GetUserPosts([FromRoute] int userId)
    {
        try
        {
            var posts = await _userService.GetUserPostsAsync(userId);
            return Ok(posts);
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDto)
    {
        try
        {
            var createdUser = _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyUser([FromBody] UserRequestDTO user)
    {
        try
        {
            var userId = GetCurrentUserId(); // Uso do Helper
            await _userService.UpdateUserAsync(userId, user);
            
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        catch (ArgumentNullException ex) { return BadRequest(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            var userId = GetCurrentUserId(); // Uso do Helper
            await _userService.DeleteUserAsync(userId); 
            
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
        catch (UnauthorizedAccessException ex) { return Unauthorized(ex.Message); }
        catch (Exception) { return Problem("An error occurred, try again later."); }
    }
}