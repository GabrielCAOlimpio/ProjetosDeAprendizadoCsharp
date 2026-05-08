using FinTrack.API.DTOs.Transactions;
using FinTrack.API.Interfaces.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers.Transaction;

[ApiController]
[Route("api/[controller]")
]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        var transaction = await _transactionService.GetTransactionByIdAsync(id);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetTransactionsByUserId(Guid userId)
    {
        var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId);
        return Ok(transactions);
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> AddTransaction(Guid userId, [FromBody] TransactionRequestDTO transactionCreateDto)
    {
        await _transactionService.AddTransactionAsync(userId, transactionCreateDto);
        return Created();
    }   

    [HttpPut("{id}/user/{userId}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, Guid userId, [FromBody] TransactionRequestDTO transactionUpdateDto)
    {
        await _transactionService.UpdateTransactionAsync(id, userId, transactionUpdateDto);
        return NoContent();
    }
    
    [HttpDelete("{id}/user/{userId}")]
    public async Task<IActionResult> DeleteTransaction(Guid id, Guid userId)
    {
        await _transactionService.DeleteTransactionAsync(id, userId);
        return NoContent();
    }


}