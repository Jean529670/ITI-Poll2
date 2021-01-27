using System;
using System.Threading.Tasks;
using ITI.Poll.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace ITI.Poll.Infrastructure
{
    public sealed class UserRepository : IUserRepository
    {
        static readonly string EmailAlreadyExistsMsg = "A user with this email already exists.";
        static readonly string NicknameAlreadyExistsMsg = "A user with this nickname already exists.";
        static readonly string UserNotFoundByEmailMsg = "Cannot find a user with this email.";
        static readonly string UserNotFoundByIdMsg = "Cannot find a user with this user id.";
        static readonly string UserNotFoundByNickNameMsg = "Cannot find a user with this nickname.";

        readonly IPollContextAccessor _pollContextAccessor;

        public UserRepository(IPollContextAccessor pollContextAccessor)
        {
            if (pollContextAccessor == null) throw new ArgumentNullException(nameof(pollContextAccessor));
            _pollContextAccessor = pollContextAccessor;
        }

        public Task<Result> Create(User user)
        {
            AsyncRetryPolicy policy =
                Policy
                    .Handle<DbUpdateException>(
                        e => e.InnerException != null
                            && (e.InnerException.Message.Contains("UK_poll_tUser_Email")
                                || e.InnerException.Message.Contains("UK_poll_tUser_Nickname")))
                    .RetryAsync();

            return policy.ExecuteAsync(() =>
                _pollContextAccessor.AcquirePollContext(async pollCtx =>
                {
                    if (await pollCtx.Users.AnyAsync(u => u.Email == user.Email))
                    {
                        return Result.CreateError(Errors.EmailAlreadyExists, EmailAlreadyExistsMsg);
                    }

                    if (await pollCtx.Users.AnyAsync(u => u.Nickname == user.Nickname))
                    {
                        return Result.CreateError(Errors.NicknameAlreadyExists, NicknameAlreadyExistsMsg);
                    }

                    pollCtx.Users.Add(user);
                    await pollCtx.SaveChangesAsync();
                    return Result.CreateSuccess();
                }));
        }

        public Task<Result<User>> FindByEmail(string email)
        {
            return _pollContextAccessor.AcquirePollContext(async pollCtx =>
            {
                User user = await pollCtx.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null) return Result.CreateError<User>(Errors.UserNotFound, UserNotFoundByEmailMsg);
                if (user.UserId < 16) return Result.CreateError<User>(Errors.UserNotFound, UserNotFoundByEmailMsg);
                return Result.CreateSuccess(user);
            });
        }

        public Task<Result<User>> FindById(int userId)
        {
            if (userId < 16) return Task.FromResult(Result.CreateError<User>(Errors.UserNotFound, UserNotFoundByIdMsg));
            return _pollContextAccessor.AcquirePollContext(async pollCtx =>
            {
                User user = await pollCtx.Users.SingleOrDefaultAsync(u => u.UserId == userId);
                if (user == null) return Result.CreateError<User>(Errors.UserNotFound, UserNotFoundByIdMsg);
                return Result.CreateSuccess(user);
            });
        }

        public Task<Result<User>> FindByNickname(string nickname)
        {
            return _pollContextAccessor.AcquirePollContext(async pollCtx =>
            {
                User user = await pollCtx.Users.SingleOrDefaultAsync(u => u.Nickname == nickname);
                if (user == null) return Result.CreateError<User>(Errors.UserNotFound, UserNotFoundByNickNameMsg);
                if (user.UserId < 16) return Result.CreateError<User>(Errors.UserNotFound, UserNotFoundByNickNameMsg);
                return Result.CreateSuccess(user);
            });
        }
    }
}