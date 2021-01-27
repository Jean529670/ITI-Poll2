using System;
using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public sealed class UserService : IUserService
    {
        static readonly string EmptyPasswordMsg = "The password must be not empty nor white space.";
        static readonly string TooShortPasswordMsg = "The password must contains at least 6 characters.";
        static readonly string AuthFailureMsg = "Email or password incorrect.";

        readonly IPasswordHasher _passwordHasher;
        readonly IUserDeletedEventHandler _userDeletedEventHandler;

        public UserService(IPasswordHasher passwordHasher, IUserDeletedEventHandler userDeletedEventHandler)
        {
            if (passwordHasher == null) throw new ArgumentNullException(nameof(passwordHasher));
            if (userDeletedEventHandler == null) throw new ArgumentNullException(nameof(userDeletedEventHandler));

            _passwordHasher = passwordHasher;
            _userDeletedEventHandler = userDeletedEventHandler;
        }

        public async Task<Result<User>> CreateUser(IUserRepository userRepository, string email, string nickname, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return Result.CreateError<User>(Errors.EmptyPassword, EmptyPasswordMsg);
            if (password.Length < 6) return Result.CreateError<User>(Errors.TooShortPassword, TooShortPasswordMsg);
            string passwordHash = _passwordHasher.HashPassword(password);

            Result<User> userResult = User.Create(email, nickname, passwordHash);
            if (!userResult.IsSuccess) return userResult;

            User user = userResult.Value;
            Result creationResult = await userRepository.Create(user);

            if (!creationResult.IsSuccess) return Result.Map<User>(creationResult);
            return Result.CreateSuccess(user);
        }

        public async Task<Result> DeleteUser(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPollRepository pollRepository,
            int userId)
        {
            Result<User> user = await userRepository.FindById(userId);
            if (!user.IsSuccess) return user;
            user.Value.Delete();
            await _userDeletedEventHandler.Handle(unitOfWork, pollRepository, userId);
            await unitOfWork.SaveChanges();
            
            return Result.CreateSuccess();
        }

        public async Task<Result<User>> Authenticate(IUserRepository userRepository, string email, string password)
        {
            Result<User> user = await userRepository.FindByEmail(email);
            if (!user.IsSuccess)
            {
                return Result.CreateError<User>(Errors.AuthFailure, AuthFailureMsg);
            }

            if(!_passwordHasher.VerifyPassword(user.Value.PasswordHash, password))
            {
                return Result.CreateError<User>(Errors.AuthFailure, AuthFailureMsg);
            }

            return user;
        }

        public Task<Result<User>> FindById(IUserRepository userRepository, int userId)
        {
            return userRepository.FindById(userId);
        }

        public Task<Result<User>> FindByNickname(IUserRepository userRepository, string nickname)
        {
            return userRepository.FindByNickname(nickname);
        }
    }
}