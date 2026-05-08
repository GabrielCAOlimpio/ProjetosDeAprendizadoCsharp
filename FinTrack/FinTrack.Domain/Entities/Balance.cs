namespace FinTrack.Domain.Entities;

public class Balance
{
    public Guid     Id        { get; private set; }
    public Guid     UserId    { get; private set; }
    public decimal  Amount    { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public User User {get; private set;} = null!;

    public Balance(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId is required.");

        Id        = Guid.NewGuid(); 
        UserId    = userId;
        Amount    = 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Credit(decimal amount) 
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be a positive number.");

        Amount    += amount;
        UpdatedAt  = DateTime.UtcNow;
    }

    public void Debit(decimal amount) 
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be a positive number.");

        if (Amount < amount)
            throw new InvalidOperationException("Insufficient balance.");

        Amount    -= amount;
        UpdatedAt  = DateTime.UtcNow;
    }
}