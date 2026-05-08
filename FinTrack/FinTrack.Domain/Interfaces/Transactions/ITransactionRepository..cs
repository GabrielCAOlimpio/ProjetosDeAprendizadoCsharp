using FinTrack.Domain.Entities;

namespace FinTrack.Domain.Interfaces.Transactions
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(Guid userId, int pageNumber = 1, int pageSize = 20);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Guid id, Transaction transaction);
        Task DeleteTransactionAsync(Guid id, Guid userId); // Recebe userId para garantir que só o dono possa deletar

    }
}