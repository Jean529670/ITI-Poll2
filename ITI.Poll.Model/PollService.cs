using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public sealed class PollService : IPollService
    {
        static readonly string InvalidAuthorIdMsg = "The author id is invalid.";
        static readonly string NotEnoughGuestsMsg = "At least 1 guest is mandatory.";
        static readonly string AuthorCannotBeGuestMsg = "The author of a poll can not be a guest of this poll.";
        static readonly string EmptyQuestionMsg = "The question must be not null nor white space.";
        static readonly string NotEnoughProposalsMsg = "At least 2 proposals are mandatory.";
        static readonly string EmptyProposalMsg = "The proposals must be not null nor white space.";
        static readonly string ForbiddenMsg = "Forbidden";

        public async Task<Result<Poll>> CreatePoll(IUnitOfWork unitOfWork, IPollRepository pollRepository, IUserRepository userRepository, NewPollDto newPollInfo)
        {
            Result<User> author = await userRepository.FindById(newPollInfo.AuthorId);
            if (!author.IsSuccess) return Result.Map<Poll>(author);

            Result validation = Validate(newPollInfo, author.Value.Nickname);
            if (!validation.IsSuccess) return Result.Map<Poll>(validation);

            Poll poll = new Poll(0, newPollInfo.AuthorId, newPollInfo.Question, false);
            Result pollCreation = await pollRepository.Create(poll);
            if (!pollCreation.IsSuccess) return Result.Map<Poll>(pollCreation);

            foreach (string proposalText in newPollInfo.Proposals) poll.AddProposal(proposalText);
            Proposal noProposal = await pollRepository.GetNoProposal();
            foreach (string guestNickname in newPollInfo.GuestNicknames)
            {
                Result<User> guest = await userRepository.FindByNickname(guestNickname);
                if (!guest.IsSuccess) return Result.Map<Poll>(guest);
                poll.AddGuest(guest.Value.UserId, noProposal);
            }

            await unitOfWork.SaveChanges();

            return Result.CreateSuccess(poll);
        }

        Result Validate(NewPollDto newPollInfo, string authorNickname)
        {
            if (newPollInfo.AuthorId < 16)
                return Result.CreateError(Errors.InvalidAuthorId, InvalidAuthorIdMsg);
            if (newPollInfo.GuestNicknames == null || newPollInfo.GuestNicknames.Length < 1)
                return Result.CreateError(Errors.NotEnoughGuests, NotEnoughGuestsMsg);
            if (newPollInfo.GuestNicknames.Select(n => n.ToLowerInvariant()).Contains(authorNickname.ToLowerInvariant()))
                return Result.CreateError(Errors.AuthorCannotBeGuest, AuthorCannotBeGuestMsg);
            if (string.IsNullOrWhiteSpace(newPollInfo.Question))
                return Result.CreateError(Errors.EmptyQuestion, EmptyQuestionMsg);
            if (newPollInfo.Proposals == null || newPollInfo.Proposals.Length < 2)
                return Result.CreateError(Errors.NotEnoughProposals, NotEnoughProposalsMsg);
            if (newPollInfo.Proposals.Any(p => string.IsNullOrWhiteSpace(p)))
                return Result.CreateError(Errors.EmptyProposal, EmptyProposalMsg);

            return Result.CreateSuccess();
        }

        public async Task<Result> DeletePoll(IUnitOfWork unitOfWork, IPollRepository pollRepository, int pollId)
        {
            Result<Poll> poll = await pollRepository.FindById(pollId);
            if (!poll.IsSuccess) return poll;

            var removed = poll.Value.Delete();
            
            pollRepository.Remove(removed);
            await unitOfWork.SaveChanges();
            return Result.CreateSuccess();
        }

        public Task<Result<Poll>> FindById(IPollRepository pollRepository, int pollId)
        {
            return pollRepository.FindById(pollId);
        }

        public async Task<Result> Answer(IUnitOfWork unitOfWork, IPollRepository pollRepository, int pollId, int userId, int proposalId)
        {
            Result<Poll> pollResult = await FindById(pollRepository, pollId);
            if (!pollResult.IsSuccess) return pollResult;

            Poll poll = pollResult.Value;
            Result answerResult = poll.Answer(userId, proposalId);
            if (!answerResult.IsSuccess) return answerResult;

            await unitOfWork.SaveChanges();

            return Result.CreateSuccess();
        }

        public Task<Result<IEnumerable<Poll>>> FindUserPolls(IPollRepository pollRepository, int userId)
        {
            return pollRepository.FindUserPolls(userId);
        }

        public async Task<Result<Poll>> FindByIdForAuthor(IPollRepository pollRepository, int authorId, int pollId)
        {
            Result<Poll> poll = await pollRepository.FindById(pollId);
            if (!poll.IsSuccess) return poll;
            if (poll.Value.AuthorId != authorId) return Result.CreateError<Poll>(Errors.Forbidden, ForbiddenMsg);
            return poll;
        }

        public Task<Result<IEnumerable<Poll>>> FindUserInvitations(IPollRepository pollRepository, int userId)
        {
            return pollRepository.FindUserInvitations(userId);
        }

        public async Task<Result<Poll>> FindByIdForGuest(IPollRepository pollRepository, int pollId, int guestId)
        {
            Result<Poll> poll = await pollRepository.FindById(pollId);
            if (!poll.IsSuccess) return poll;
            if (!poll.Value.Guests.Any(g => g.UserId == guestId)) return Result.CreateError<Poll>(Errors.Forbidden, ForbiddenMsg);
            return poll;
        }

        public async Task<Result> DeleteGuest(IPollRepository pollRepository, int guestId, int pollId)
        {
            Result<Poll> poll = await pollRepository.FindById(pollId);
            if (!poll.IsSuccess) return poll;
            return poll.Value.RemoveGuest(guestId);
        }
    }
}