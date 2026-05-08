namespace FinTrack.API.DTOs.Transactions
{
    public class TransactionResponseDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Type { get; set; } // 0 for expense, 1 for income
        public DateTime Date { get; set; }
    }
}
