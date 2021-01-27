namespace ITI.Poll.Model
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string passwordHash, string candidate);
    }
}