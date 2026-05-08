namespace FinTrack.Domain.Entities
{
    public class User
    {
        public Guid Id {get; private set; }
        public string Name {get; private set; } = string.Empty;
        public string Email {get; private set;} = string.Empty;
        public string PasswordHash {get; private set;} = string.Empty;
        public DateTime CreatedAt {get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public Balance Balance {get; private set;} = null!;
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        private User() { }

        public User(string? name, string? email, string? passwordHash)
        {
            name = name?.Trim();
            email = email?.Trim();
            passwordHash = passwordHash?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
            
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            
        }
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}