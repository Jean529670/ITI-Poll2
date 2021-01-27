using ITI.Poll.Model;

namespace ITI.Poll.AspNetCore
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        Microsoft.AspNetCore.Identity.PasswordHasher<object> _passwordHasher;

        public PasswordHasher()
        {
            _passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<object>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string passwordHash, string candidate)
        {
            return _passwordHasher.VerifyHashedPassword(null, passwordHash, candidate)
                != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed;
        }
    }
}