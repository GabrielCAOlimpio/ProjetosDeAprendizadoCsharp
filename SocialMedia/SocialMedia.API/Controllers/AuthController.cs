using Microsoft.AspNetCore.Mvc;
using SocialMedia.Domain.DTOs.Auth;
using SocialMedia.Domain.Interfaces.Auth;
using SocialMedia.Domain.DTOs.Users;
using Asp.Versioning;

namespace SocialMedia.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/auth")]
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
        try
        {
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto), "Registration data cannot be null.");

            var result = await _authService.Register(registerDto);
            
            // Retorna 201 Created
            return CreatedAtAction(nameof(Register), new { email = result.Email }, result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) // Caso e-mail já exista, por exemplo
        {
            return Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
    {
        try
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto), "Login data cannot be null.");

            var token = await _authService.Login(loginDto);
            
            return Ok(token);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex) // Caso senha esteja errada
        {
            return Unauthorized(ex.Message);
        }
        catch (KeyNotFoundException ex) // Caso usuário não exista
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred during login, try again later.");
        }
    }
}