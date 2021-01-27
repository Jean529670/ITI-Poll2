using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public sealed class UserDeletedEventHandler : IUserDeletedEventHandler
    {
        readonly IPollService _pollService;

        public UserDeletedEventHandler(IPollService pollService)
        {
            if (pollService == null) throw new ArgumentNullException(nameof(pollService));
            _pollService = pollService;
        }

        public async Task Handle(IUnitOfWork unitOfWork, IPollRepository pollRepository, int userId)
        {
            Result<IEnumerable<Poll>> polls = await _pollService.FindUserPolls(pollRepository, userId);
            if (polls.IsSuccess)
            {
                foreach (Poll poll in polls.Value)
                {
                    await _pollService.DeletePoll(unitOfWork, pollRepository, poll.PollId);
                }
            }

            polls =  await _pollService.FindUserInvitations(pollRepository, userId);
            if (polls.IsSuccess)
            {
                foreach (Poll poll in polls.Value)
                {
                    await _pollService.DeleteGuest(pollRepository, userId, poll.PollId);
                }
            }
        }
    }
}