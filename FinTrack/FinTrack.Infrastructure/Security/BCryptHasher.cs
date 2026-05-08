namespace FinTrack.Infrastructure.Security
{
    using FinTrack.Domain.Interfaces.Security;

    public class BCryptHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
            
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty or whitespace.", nameof(passwordHash));
            
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}