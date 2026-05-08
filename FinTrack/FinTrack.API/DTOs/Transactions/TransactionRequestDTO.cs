using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinTrack.API.DTOs.Transactions
{
    public class TransactionRequestDTO
    {
        [Required]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        [DefaultValue(0)]
        public decimal Amount { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Type must be 0 for expense or 1 for income.")]
        public int Type { get; set; } // 0 for expense, 1 for income
        
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }
    }
}
