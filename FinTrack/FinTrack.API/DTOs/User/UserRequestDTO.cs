
namespace FinTrack.API.DTOs.User
{
    using System.ComponentModel.DataAnnotations;
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Nome não pode ser maior que 100 caracteres!")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório!")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória!")]
        [StringLength(100, ErrorMessage = "Senha deve ter no máximo 100 caracteres!", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}