using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Domain.DTOs.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
        if (pageNumber <= 0 || pageSize <= 0)
            throw new ArgumentException("Page number and page size must be positive integers.");
        
        var users = await _userService.GetAllAsync(pageNumber, pageSize);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);    
    }

    [HttpGet("{userId}/posts")]
    public async Task<IActionResult> GetUserPosts([FromRoute] int userId)
    {
        var posts = await _userService.GetUserPostsAsync(userId);
        return Ok(posts);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userDto)
    {
        await _userService.CreateUserAsync(userDto);
        return Created();
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyUser([FromBody] UserRequestDTO user)
    {
        var userId = GetCurrentUserId(); // Uso do Helper
        await _userService.UpdateUserAsync(userId, user);
        
        return NoContent();
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = GetCurrentUserId(); // Uso do Helper
        await _userService.DeleteUserAsync(userId); 
        
        return NoContent();
    }
}