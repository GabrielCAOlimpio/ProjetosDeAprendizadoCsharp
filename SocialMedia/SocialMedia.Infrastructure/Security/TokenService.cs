using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces.Auth;

namespace SocialMedia.Infrastructure.Security.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration config) => _configuration = config;

    public string GenerateToken(User user)
    {
        var secret = _configuration["JwtSettings:Secret"];

        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JWT Secret Key is missing in appsettings.json.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username), //ClaimTypes.Name é string, é o identificador do nome
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), //.NameIdentifier é como vai identificar aquele user
            new Claim(ClaimTypes.Email, user.Email), //.Email verifica o email e salva do usuario    
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}