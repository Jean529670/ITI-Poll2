using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.Poll.Model;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace ITI.Poll.Infrastructure
{
    public sealed class PollRepository : IPollRepository
    {
        static readonly string UnknownAuthorIdMsg = "The author id is unknown.";
        static readonly string InvalidIdMsg = "This poll id is invalid.";
        static readonly string PollNotFoundMsg = "This poll is unknown.";
        static readonly string InvalidUserIdMsg = "This user id is invalid.";

        readonly IPollContextAccessor _pollContextAccessor;

        public PollRepository(IPollContextAccessor pollContextAccessor)
        {
            if (pollContextAccessor == null) throw new ArgumentNullException(nameof(pollContextAccessor));
            _pollContextAccessor = pollContextAccessor;
        }

        public Task<Result> Create(ITI.Poll.Model.Poll poll)
        {
            AsyncRetryPolicy policy =
                Policy
                    .Handle<DbUpdateException>(
                        e => e.InnerException != null
                            && e.InnerException.Message.Contains("FK_poll_tPoll_AuthorId"))
                    .RetryAsync();

            return policy.ExecuteAsync(() =>
                _pollContextAccessor.AcquirePollContext(async pollCtx =>
                {
                    if (!await pollCtx.Users.AnyAsync(u => u.UserId == poll.AuthorId))
                    {
                        return Result.CreateError(Errors.UnknownAuthorId, UnknownAuthorIdMsg);
                    }

                    pollCtx.Polls.Add(poll);
                    await pollCtx.SaveChangesAsync();
                    return Result.CreateSuccess();
                }));
        }

        public async Task<Result<ITI.Poll.Model.Poll>> FindById(int pollId)
        {
            if (pollId < 16) return Result.CreateError<ITI.Poll.Model.Poll>(Errors.InvalidId, InvalidIdMsg);

            return await _pollContextAccessor.AcquirePollContext(async pollCtx =>
            {
                var poll = await pollCtx.Polls
                    .Include(p => p.Guests)
                        .ThenInclude(g => g.Vote)
                    .Include(p => p.Proposals)
                    .SingleOrDefaultAsync(p => p.PollId == pollId);

                if (poll == null) return Result.CreateError<ITI.Poll.Model.Poll>(Errors.PollNotFound, PollNotFoundMsg);
                return Result.CreateSuccess(poll);
            });
        }

        public async Task<Result<IEnumerable<Model.Poll>>> FindUserInvitations(int userId)
        {
            if (userId < 16) return Result.CreateError<IEnumerable<Model.Poll>>(Errors.InvalidUserId, InvalidUserIdMsg);

            return await _pollContextAccessor.AcquirePollContext(async pollCtx =>
            {
                IEnumerable<Model.Poll> polls = await pollCtx.Guests
                    .Include(g => g.Poll)
                    .Where(g => g.UserId == userId && g.Vote.ProposalId == 0)
                    .Select(g => g.Poll)
                    .ToListAsync();

                return Result.CreateSuccess(polls);
            });
        }

        public async Task<Result<IEnumerable<Model.Poll>>> FindUserPolls(int userId)
        {
            if (userId < 16) return Result.CreateError<IEnumerable<Model.Poll>>(Errors.InvalidUserId, InvalidUserIdMsg);

            return await _pollContextAccessor.AcquirePollContext(async pollCtx =>
            {
                IEnumerable<Model.Poll> polls = await pollCtx.Polls.Where(p => p.AuthorId == userId).ToListAsync();
                return Result.CreateSuccess(polls);
            });
        }

        public Task<Proposal> GetNoProposal()
        {
            return _pollContextAccessor.AcquirePollContext(pollCtx => pollCtx.Proposals.SingleAsync(p => p.ProposalId == 0));
        }

        public void Remove(IEnumerable<object> toRemove)
        {
            _pollContextAccessor.AcquirePollContext(pollCtx =>
            {
                pollCtx.RemoveRange(toRemove);
            });
        }
    }
}