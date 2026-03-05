using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialMedia.Domain.DTOs.Users;
using SocialMedia.Domain.Entities;
using SocialMedia.Domain.Interfaces.Users;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SocialMediaContext _context;

    public UserRepository(SocialMediaContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync(int pages = 1, int pageSize = 10)
    {
        var users = await _context.Users.AsNoTracking().Where(u => u.IsActive).Skip((pages - 1) * pageSize).Take(pageSize).ToListAsync();

        if (users == null || users.Count == 0)
        {
            throw new KeyNotFoundException("No users found.");
        }

        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id && u.IsActive);
        
        if (user != null)
        {  
            return user;
        }

        throw new KeyNotFoundException($"User with ID {id} not found.");
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public async Task CreateAsync(User user)
    {
        var repeatedNameOrEmail = await _context.Users.AsNoTracking()
            .Where(u => u.Username == user.Username || u.Email == user.Email)
            .ToListAsync();
        
        if (repeatedNameOrEmail.Count > 0)
        {
            if (repeatedNameOrEmail.Any(u => u.Username == user.Username))
                throw new InvalidOperationException($"Username '{user.Username}' is already existing.");
            
            throw new InvalidOperationException($"Email '{user.Email}' is already existing.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var repeatedNameOrEmail = await _context.Users.AsNoTracking()
            .Where(u => (u.Username == user.Username || u.Email == user.Email) && u.UserId != user.UserId)
            .ToListAsync();
        
        if (repeatedNameOrEmail.Count > 0)
        {
            if (repeatedNameOrEmail.Any(u => u.Username == user.Username))
                throw new InvalidOperationException($"Username '{user.Username}' is already existing.");
            
            throw new InvalidOperationException($"Email '{user.Email}' is already existing.");
        }

        _context.Users.Update(user);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        } 

        existingUser.IsActive = false;

        await _context.SaveChangesAsync();
    }

    public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .ToListAsync();

        if (posts == null || posts.Count == 0)
        {
            throw new KeyNotFoundException($"No posts found for user with ID {userId}.");
        }

        return posts;
    }
}