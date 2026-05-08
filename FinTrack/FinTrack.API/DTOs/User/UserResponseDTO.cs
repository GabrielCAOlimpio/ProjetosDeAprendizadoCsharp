namespace FinTrack.API.DTOs.User
{
    using System.ComponentModel.DataAnnotations;
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}