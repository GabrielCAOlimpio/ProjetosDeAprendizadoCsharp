

namespace FinTrack.Infrastructure.Repositories.User
{
    using FinTrack.Domain.Entities;
    using FinTrack.Domain.Interfaces.User;
    using FinTrack.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly FinTrackDbContext _context;

        public UserRepository(FinTrackDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.Balances.AddAsync(new Balance(user.Id));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => 
                    u.SetProperty(x => x.IsActive, false)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Balance)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Balance)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserNameAsync(Guid id, string newName)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => 
                    u.SetProperty(x => x.Name, newName)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );
        }
        public async Task UpdateUserEmailAsync(Guid id, string newEmail)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => 
                    u.SetProperty(x => x.Email, newEmail)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );
        }
        public async Task UpdateUserPasswordAsync(Guid id, string newPasswordHash)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => 
                    u.SetProperty(x => x.PasswordHash, newPasswordHash)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );
        }
        public async Task RecoverUserAsync(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => 
                    u.SetProperty(x => x.IsActive, true)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );
        }
        
    }
}