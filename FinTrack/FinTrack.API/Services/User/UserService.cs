using FinTrack.API.DTOs.User;
using FinTrack.API.Interfaces.Balances;
using FinTrack.API.Interfaces.User;
using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Security;
using FinTrack.Domain.Interfaces.User;

namespace FinTrack.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;


        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        // ─── Métodos privados de apoio ────────────────────────────────────────

        private static void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID do usuário não pode ser vazio.", nameof(id));
        }

        private async Task<User> GetUserOrThrowAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Usuário com ID '{id}' não encontrado.");
            if (!user.IsActive)
                throw new InvalidOperationException("Usuário foi deletado recentemente!");
            
            return user;
        }
        private async Task<User> GetInactiveUserOrThrowAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"Usuário com ID '{id}' não encontrado.");
            if (user.IsActive)
                throw new InvalidOperationException("Usuário já está ativo.");
            return user;
        }

        private static UserResponseDTO ToResponseDTO(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            Balance = user.Balance.Amount
        };

        // - METODOS DO SERVICE

        public async Task<UserResponseDTO> GetUserByIdAsync(Guid id)
        {
            ValidateId(id);
            var user = await GetUserOrThrowAsync(id);
            
            return ToResponseDTO(user);
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio.", nameof(email));

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"Usuário com email '{email}' não encontrado.");
            if (!user.IsActive)
                throw new InvalidOperationException("Usuário foi deletado recentemente!");

            return ToResponseDTO(user);
        }

        public async Task AddUserAsync(UserRequestDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto), "Dados do usuário não podem ser nulos.");
            var existing = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email já está em uso.");

            var hash = _passwordHasher.HashPassword(userDto.Password);
            var user = new User(userDto.Name.Trim(), userDto.Email.Trim(), hash);
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserNameAsync(Guid id, string newName)
        {
            ValidateId(id);
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Novo nome não pode ser vazio.", nameof(newName));

            await GetUserOrThrowAsync(id);
            await _userRepository.UpdateUserNameAsync(id, newName.Trim());
        }

        public async Task UpdateUserEmailAsync(Guid id, string newEmail)
        {
            ValidateId(id);
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Novo email não pode ser vazio.", nameof(newEmail));

            await GetUserOrThrowAsync(id);

            var existing = await _userRepository.GetUserByEmailAsync(newEmail);
            if (existing != null)
                throw new InvalidOperationException("Email já está em uso.");

            await _userRepository.UpdateUserEmailAsync(id, newEmail.Trim());
        }

        public async Task UpdateUserPasswordAsync(Guid id, string newPassword)
        {
            ValidateId(id);
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Nova senha não pode ser vazia.", nameof(newPassword));

            await GetUserOrThrowAsync(id);

            var newHashedPassword = _passwordHasher.HashPassword(newPassword);
            await _userRepository.UpdateUserPasswordAsync(id, newHashedPassword);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            ValidateId(id);
            await GetUserOrThrowAsync(id);
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task RecoverUserAsync(Guid id)
        {
            ValidateId(id);
            await GetInactiveUserOrThrowAsync(id);
            await _userRepository.RecoverUserAsync(id);
        }
    }
}