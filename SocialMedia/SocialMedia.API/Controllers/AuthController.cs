using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.DTOs.Auth;
using SocialMedia.Domain.Interfaces.Auth;
using SocialMedia.Domain.DTOs.Users;
using Asp.Versioning;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDto)
    {
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Registration data cannot be null.");

        var result = await _authService.Register(registerDto);

        return CreatedAtAction(nameof(Register), new { email = result.Email }, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
    {
        if (loginDto == null)
            throw new ArgumentNullException(nameof(loginDto), "Login data cannot be null.");

        var token = await _authService.Login(loginDto);

        return Ok(token);
    }
}