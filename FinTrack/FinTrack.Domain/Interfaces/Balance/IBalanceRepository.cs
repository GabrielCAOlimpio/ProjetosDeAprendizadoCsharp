namespace FinTrack.Domain.Interfaces.Balances
{
    using FinTrack.Domain.Entities;
    public interface IBalanceRepository
    {
        Task<Balance?> GetBalanceByUserIdAsync(Guid userId);
        Task UpdateBalanceAsync(Balance balance);
    }   
}
