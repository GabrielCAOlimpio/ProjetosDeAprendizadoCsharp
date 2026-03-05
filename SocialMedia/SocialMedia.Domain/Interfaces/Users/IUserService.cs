using SocialMedia.Domain.DTOs.Users;
using SocialMedia.Domain.DTOs.Posts;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces.Users;

public interface IUserService
{
    public Task<List<UserResponseDTO>> GetAllAsync(int pages = 1, int pageSize = 10);
    public Task<UserResponseDTO> GetUserByIdAsync(int userId);
    public Task<UserResponseDTO> GetByEmailAsync(string email);
    public Task CreateUserAsync(UserRequestDTO user);
    public Task UpdateUserAsync(int userId, UserRequestDTO user);
    public Task DeleteUserAsync(int userId);
    public Task<List<PostsResponseDTO>> GetUserPostsAsync(int userId);
}