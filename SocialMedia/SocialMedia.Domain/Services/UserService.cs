using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Domain.DTOs.Users;
using SocialMedia.Domain.DTOs.Posts;

namespace SocialMedia.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponseDTO>> GetAllAsync(int pages, int pageSize)
    {
        if (pages <= 0 || pageSize <= 0)
            throw new ArgumentException("Page number and page size must be positive integers.");

        if (pageSize > 100)
            throw new ArgumentException("Page size cannot exceed 100.");

        List<User> users = await _userRepository.GetAllAsync(pages, pageSize);

        return users.Where(u => u.IsActive).Select(u => new UserResponseDTO()
        {
            Username = u.Username,
            Email = u.Email,
            Bio = u.Bio
        }).ToList();
    }

    public async Task<UserResponseDTO> GetUserByIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.");
        
        User user = await _userRepository.GetByIdAsync(userId);

        return new UserResponseDTO()
        {
            Username = user.Username,
            Email = user.Email,
            Bio = user.Bio
        };
    }

    public async Task<UserResponseDTO> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required and cannot be empty.");

        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            throw new KeyNotFoundException("No user found with the provided email address.");

        return new UserResponseDTO()
        {
            Username = user.Username,
            Email = user.Email,
            Bio = user.Bio
        };
    }

    public async Task CreateUserAsync(UserRequestDTO user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");

        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email cannot be empty.");

        var newUser = new User()
        {
            Username = user.Username,
            Email = user.Email,
            Bio = user.Bio,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userRepository.CreateAsync(newUser);
    }

    public async Task UpdateUserAsync(int userId, UserRequestDTO user)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.");
        
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username cannot be empty.");
        
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email cannot be empty.");
        
        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Bio = user.Bio;

        await _userRepository.UpdateAsync(existingUser);
    }

    public async Task DeleteUserAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.");

        await _userRepository.DeleteAsync(userId);
    }

    public async Task<List<PostsResponseDTO>> GetUserPostsAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.");

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        var posts = await _userRepository.GetPostsByUserIdAsync(userId);

        if (posts == null || posts.Count == 0)
            throw new KeyNotFoundException($"No posts found for user with ID {userId}.");

        var userDto = new UserResponseDTO
        {
            Username = user.Username,
            Email = user.Email,
            Bio = user.Bio
        };

        var postItems = posts.Select(p => new PostItemDTO
        {
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            LikesCount = p.LikesCount
        }).ToList();

        return new List<PostsResponseDTO>
        {
            new PostsResponseDTO
            {
                User = userDto,
                Posts = postItems
            }
        };
    }
}