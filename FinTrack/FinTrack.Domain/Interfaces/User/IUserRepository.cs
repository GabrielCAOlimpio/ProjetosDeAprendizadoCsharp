namespace FinTrack.Domain.Interfaces.User
{
    using FinTrack.Domain.Entities;
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid id);
        Task AddUserAsync(User user);
        Task UpdateUserNameAsync(Guid id, string newName);
        Task UpdateUserEmailAsync(Guid id, string newEmail);
        Task UpdateUserPasswordAsync(Guid id, string newPasswordHash);
        Task DeleteUserAsync(Guid id);
        Task RecoverUserAsync(Guid id);
    }
}