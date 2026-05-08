using FinTrack.API.DTOs.Balance;
using FinTrack.Domain.Entities;

namespace FinTrack.API.Interfaces.Balances
{
    public interface IBalanceService
    {
        Task<Balance> GetBalanceByUserIdAsync(Guid userId);
        Task CreditAsync(BalanceRequestDTO requestDTO); // ← chamado pelo TransactionService
        Task DebitAsync(BalanceRequestDTO requestDTO);  // ← chamado pelo TransactionService
    }
}