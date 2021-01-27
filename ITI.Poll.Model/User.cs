using System;
using System.Text.RegularExpressions;

namespace ITI.Poll.Model
{
    public sealed class User
    {
        static readonly string InvalidEmailMsg = "The email is invalid.";
        static readonly string InvalidNickNameMsg = "The nickname is invalid.";

        static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public User(
            int userId,
            string email,
            string nickname,
            string passwordHash,
            bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("The email must be not null nor white space.", nameof(email));
            if (string.IsNullOrWhiteSpace(nickname)) throw new ArgumentException("The nickname must be not null nor white space.", nameof(nickname));
            if (isDeleted && passwordHash != string.Empty) throw new ArgumentException("The password of a deleted user must be empty.");
            if (!isDeleted && string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("The password of a not deleted user must be not null nor white space.");
            if (!isDeleted && !IsEmailValid(email)) throw new ArgumentException("The email is not valid.", nameof(email));

            UserId = userId;
            Email = email;
            Nickname = nickname;
            PasswordHash = passwordHash;
            IsDeleted = isDeleted;
        }

        public int UserId { get; private set; }

        public string Email { get; private set; }

        public string Nickname { get; private set; }

        public string PasswordHash { get; private set; }

        public bool IsDeleted { get; private set; }

        public void Delete()
        {
            if (IsDeleted) return;

            IsDeleted = true;
            Email = $"Deleted-{Guid.NewGuid()}";
            Nickname = $"Deleted-{Guid.NewGuid()}";
            PasswordHash = string.Empty;
        }

        public static Result<User> Create(string email, string nickname, string passwordHash)
        {
            if (email == null || !IsEmailValid(email)) return Result.CreateError<User>(Errors.InvalidEmail, InvalidEmailMsg);
            if (string.IsNullOrWhiteSpace(nickname)) return Result.CreateError<User>(Errors.InvalidNickName, InvalidNickNameMsg);

            return Result.CreateSuccess(new User(0, email, nickname, passwordHash, false));
        }

        static bool IsEmailValid(string email)
        {
            return EmailRegex.IsMatch(email);
        }
    }
}