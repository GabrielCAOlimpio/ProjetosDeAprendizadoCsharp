using FinTrack.API.DTOs.Transactions;

namespace FinTrack.API.Interfaces.Transaction
{
    public interface ITransactionService
    {
        Task<TransactionResponseDTO> GetTransactionByIdAsync(Guid id);
        Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByUserIdAsync(Guid userId);
        Task AddTransactionAsync(Guid userId,TransactionRequestDTO transactionCreateDto);
        Task UpdateTransactionAsync(Guid id, Guid userId, TransactionRequestDTO transactionUpdateDto);
        Task DeleteTransactionAsync(Guid id, Guid userId);
    }
}