namespace SocialMedia.Domain.Services;
using SocialMedia.Domain.DTOs.Auth;
using SocialMedia.Domain.Interfaces.Auth;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Domain.DTOs.Users;
using SocialMedia.Domain.Entities;

public class AuthService : IAuthService 
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        ITokenService tokenService) 
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<UserResponseDTO> Register(RegisterRequestDTO dto)
    {
        // 1. Validate if user already exists
        var existing = await _userRepository.GetByEmailAsync(dto.Email);
        if (existing != null) 
            throw new InvalidOperationException("User with this email already exists.");

        // 2. Validate password match
        if (dto.Password != dto.RepeatPassword)
            throw new ArgumentException("Passwords do not match.");

        // 3. Hash password and create entity
        var newUser = new User {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // 4. Save via Repository
        await _userRepository.CreateAsync(newUser);

        return new UserResponseDTO 
        { 
            Username = newUser.Username, 
            Email = newUser.Email, 
            Bio = newUser.Bio 
        };
    }

    public async Task<TokenResponseDTO> Login(LoginRequestDTO dto) 
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        
        // Use a generic message for security reasons (don't tell if the email or password is wrong)
        if (user == null || !_passwordHasher.Verify(user.PasswordHash, dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = new TokenResponseDTO()
        {
            Token = _tokenService.GenerateToken(user)
        };

        return token;
    }
}