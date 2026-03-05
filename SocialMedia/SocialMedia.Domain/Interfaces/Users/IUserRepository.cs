using SocialMedia.Domain.DTOs.Users;
using SocialMedia.Domain.Entities;

namespace SocialMedia.Domain.Interfaces.Users;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(int pages = 1, int pageSize = 10);
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmailAsync(string email);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<List<Post>> GetPostsByUserIdAsync(int userId);
}