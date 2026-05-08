using FinTrack.API.Interfaces.Balances;
using FinTrack.API.DTOs.Balance;
using FinTrack.Domain.Entities;
using FinTrack.Domain.Interfaces.Balances;
using FinTrack.Domain.Interfaces.User;

namespace FinTrack.API.Services.Balances;

public class BalanceService : IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IUserRepository _userRepository; 

    public BalanceService(IBalanceRepository balanceRepository, IUserRepository userRepository)
    {
        _balanceRepository = balanceRepository;
        _userRepository    = userRepository;
    }

    public async Task<Balance> GetBalanceByUserIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId)
            ?? throw new KeyNotFoundException("User not found.");

        return await _balanceRepository.GetBalanceByUserIdAsync(userId)
            ?? throw new KeyNotFoundException("Balance not found.");
    }

    public async Task CreditAsync(BalanceRequestDTO dto)
    {
        var balance = await _balanceRepository.GetBalanceByUserIdAsync(dto.UserId)
            ?? throw new KeyNotFoundException("Balance not found.");

        balance.Credit(dto.Amount); 
        await _balanceRepository.UpdateBalanceAsync(balance);
    }

    public async Task DebitAsync(BalanceRequestDTO dto)
    {
        var balance = await _balanceRepository.GetBalanceByUserIdAsync(dto.UserId)
            ?? throw new KeyNotFoundException("Balance not found.");

        balance.Debit(dto.Amount); // ✅ entidade valida e aplica
        await _balanceRepository.UpdateBalanceAsync(balance);
    }
}