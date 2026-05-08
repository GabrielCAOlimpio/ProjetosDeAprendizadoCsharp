namespace FinTrack.Infrastructure.Repositories.Transaction;

using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Transactions;
using FinTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class TransactionRepository : ITransactionRepository
{
    private readonly FinTrackDbContext _context;

    public TransactionRepository(FinTrackDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        return await _context.Transactions
            .AsNoTracking() // ✅ leitura — não precisa de tracking
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(
        Guid userId,
        int pageNumber = 1,  // ✅ paginação
        int pageSize   = 20)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .Skip((pageNumber - 1) * pageSize) 
            .Take(pageSize)                    
            .ToListAsync();
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTransactionAsync(Guid id, Transaction transaction)
    {
        await _context.Transactions
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(t =>
                t.SetProperty(x => x.Title,       transaction.Title)       // ✅
                 .SetProperty(x => x.Amount,      transaction.Amount)
                 .SetProperty(x => x.Description, transaction.Description)
                 .SetProperty(x => x.Type,        transaction.Type)        // ✅
                 .SetProperty(x => x.Date,        transaction.Date)
            );

    }

    // ✅ Recebe userId para o filtro correto
    public async Task DeleteTransactionAsync(Guid id, Guid userId)
    {
        await _context.Transactions
            .Where(t => t.Id == id && t.UserId == userId) // ✅ fix do bug
            .ExecuteDeleteAsync();
    }
}