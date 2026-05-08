namespace FinTrack.API.Interfaces.User
{
    using FinTrack.API.DTOs.User;
    using FinTrack.Domain.Entities;

    public interface IUserService
    {
        Task<UserResponseDTO> GetUserByEmailAsync(string email);
        Task<UserResponseDTO> GetUserByIdAsync(Guid id);
        Task AddUserAsync(UserRequestDTO user);
        Task UpdateUserNameAsync(Guid id, string newName);
        Task UpdateUserEmailAsync(Guid id, string newEmail);
        Task UpdateUserPasswordAsync(Guid id, string newPassword);
        Task DeleteUserAsync(Guid id);
        Task RecoverUserAsync(Guid id);
    }
}