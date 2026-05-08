namespace FinTrack.Domain.Entities
{
    public class Transaction
    {
        public Guid Id {get; private set; }
        public Guid UserId {get; private set; }
        public string Title {get; private set; } = string.Empty;
        public string Description {get; private set; } = string.Empty;  
        public decimal Amount {get; private set; }
        public int Type {get; private set; } // 0 for expense, 1 for income
        public DateTime Date {get; private set; }
        public User User { get; private set; } = null!;

        private Transaction() { }

        public Transaction(Guid userId, string? title, string? description, int type, decimal amount, DateTime date)
        {
            title = title?.Trim();
            description = description?.Trim();

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (type != 0 && type != 1)
                throw new ArgumentException("Type must be either 0 (expense) or 1 (income).", nameof(type));
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
            if (date > DateTime.UtcNow)
                throw new ArgumentException("Date cannot be in the future.", nameof(date));
            if (date < DateTime.UtcNow.AddYears(-10))
                throw new ArgumentException("Date cannot be more than 10 years in the past.", nameof(date));
            if (userId == Guid.Empty)
                throw new ArgumentException("UserID is Required");
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title;
            Description = description ?? string.Empty;
            Amount = amount;
            Date = date;
            Type = type;
        }
        
        public void UpdateTransaction(string? title, string? description, int type,decimal amount, DateTime date)
        {
            title = title?.Trim();
            description = description?.Trim();

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (type != 0 && type != 1)
                throw new ArgumentException("Type must be either 0 (expense) or 1 (income).", nameof(type));
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
            if (date > DateTime.UtcNow)
                throw new ArgumentException("Date cannot be in the future.", nameof(date));
            if (date < DateTime.UtcNow.AddYears(-10))
                throw new ArgumentException("Date cannot be more than 10 years in the past.", nameof(date));
            
            Title = title;
            Description = description ?? string.Empty;
            Amount = amount;
            Date = date;
            Type = type;
        }
    }
}