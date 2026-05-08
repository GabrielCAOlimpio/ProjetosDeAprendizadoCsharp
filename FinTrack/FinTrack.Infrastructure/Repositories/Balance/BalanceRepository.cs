using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Balances;
using FinTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Infrastructure.Repositories.Balances;

public class BalanceRepository : IBalanceRepository
{
    private readonly FinTrackDbContext _context;

    public BalanceRepository(FinTrackDbContext context)
    {
        _context = context;
    }

    public async Task<Balance?> GetBalanceByUserIdAsync(Guid userId)
    {
        return await _context.Balances
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task UpdateBalanceAsync(Balance balance)
    {
        await _context.Balances
            .Where(b => b.UserId == balance.UserId)
            .ExecuteUpdateAsync(b =>
                b.SetProperty(x => x.Amount,    balance.Amount)
                 .SetProperty(x => x.UpdatedAt, DateTime.UtcNow));
    }
}