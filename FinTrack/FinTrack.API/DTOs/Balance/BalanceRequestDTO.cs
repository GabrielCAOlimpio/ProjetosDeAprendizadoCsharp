using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs.Balance;

public class BalanceRequestDTO
{
    [Required(ErrorMessage = "UserID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    public decimal Amount { get; set; }
    
}