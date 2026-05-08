namespace FinTrack.API.Services.Transactions;

using FinTrack.API.DTOs.Transactions;
using FinTrack.API.Interfaces.Balances;
using FinTrack.API.Interfaces.Transaction;
using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Transactions;
using FinTrack.Domain.Interfaces.User;
using FinTrack.API.DTOs.Balance;


public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBalanceService _balanceService;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IBalanceService balanceService)
    {
        _transactionRepository = transactionRepository;
        _userRepository        = userRepository;
        _balanceService        = balanceService;
    }

    public async Task AddTransactionAsync(Guid userId, TransactionRequestDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

       
        var user = await _userRepository.GetUserByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        
        var transaction = new Transaction(
            userId,
            dto.Title,
            dto.Description,
            dto.Type,
            dto.Amount,
            dto.Date);
        
        if (dto.Type == 0)
            await _balanceService.DebitAsync(new BalanceRequestDTO { UserId = userId, Amount = dto.Amount });
        else
            await _balanceService.CreditAsync(new BalanceRequestDTO { UserId = userId, Amount = dto.Amount });

        await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task DeleteTransactionAsync(Guid id, Guid userId)
    {
        var transaction = await _transactionRepository.GetTransactionByIdAsync(id)
            ?? throw new KeyNotFoundException($"Transaction {id} not found.");
    
        if (transaction.UserId != userId)
            throw new UnauthorizedAccessException("You can't delete a transaction from other user!");

        if (transaction.Type == 0)
            await _balanceService.CreditAsync(new BalanceRequestDTO { UserId = userId, Amount = transaction.Amount });
        else
            await _balanceService.DebitAsync(new BalanceRequestDTO { UserId = userId, Amount = transaction.Amount });

        await _transactionRepository.DeleteTransactionAsync(id, userId);
    }

    public async Task<TransactionResponseDTO> GetTransactionByIdAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetTransactionByIdAsync(id)
            ?? throw new KeyNotFoundException($"Transaction {id} not found.");

        var user = await _userRepository.GetUserByIdAsync(transaction.UserId);

        return MapToResponse(transaction, user?.Name ?? "Unknown");
    }

    public async Task<IEnumerable<TransactionResponseDTO>> GetTransactionsByUserIdAsync(Guid userId)
    {
    
        var user = await _userRepository.GetUserByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId);

        return transactions.Select(t => MapToResponse(t, user.Name)).ToList();
    }

    public async Task UpdateTransactionAsync(Guid id, Guid userId, TransactionRequestDTO dto) 
    {
        ArgumentNullException.ThrowIfNull(dto);

        var transaction = await _transactionRepository.GetTransactionByIdAsync(id)
            ?? throw new KeyNotFoundException($"Transaction {id} not found.");

        
        if (transaction.UserId != userId)
            throw new UnauthorizedAccessException("You can't update a transaction from other user!");

        var updated = new Transaction(
            userId,
            dto.Title,
            dto.Description,
            dto.Type,
            dto.Amount,
            dto.Date);
        
        if (transaction.Type != dto.Type || transaction.Amount != dto.Amount)
        {
            // Revert old transaction
            if (transaction.Type == 0)
                await _balanceService.CreditAsync(new BalanceRequestDTO { UserId = userId, Amount = transaction.Amount });
            else
                await _balanceService.DebitAsync(new BalanceRequestDTO { UserId = userId, Amount = transaction.Amount });

            // Apply new transaction
            if (dto.Type == 0)
                await _balanceService.DebitAsync(new BalanceRequestDTO { UserId = userId, Amount = dto.Amount });
            else
                await _balanceService.CreditAsync(new BalanceRequestDTO { UserId = userId, Amount = dto.Amount });
        }


        await _transactionRepository.UpdateTransactionAsync(id, updated);
    }

    
    private static TransactionResponseDTO MapToResponse(Transaction t, string username) =>
        new()
        {
            Id          = t.Id,
            Username    = username,
            Title       = t.Title,
            Type        = t.Type,
            Amount      = t.Amount,
            Date        = t.Date,
            Description = t.Description
        };
}