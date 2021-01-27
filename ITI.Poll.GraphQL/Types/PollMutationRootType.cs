using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using ITI.Poll.Infrastructure;
using ITI.Poll.Model;
using Microsoft.AspNetCore.Http;

namespace ITI.Poll.GraphQL.Types
{
    public class PollMutationRoot
    {
        public async Task<PollPayload> CreatePoll(
            PollInput poll,
            [Service] IPollService pollService,
            [Service] IPollRepository pollRepository,
            [Service] IUserRepository userRepository,
            [Service] PollContext pollContext,
            [Service] IHttpContextAccessor httpContextAccessor)
        {

            Result<Model.Poll> createdPoll = await pollService.CreatePoll(pollContext, pollRepository, userRepository, new NewPollDto
            {
                AuthorId = httpContextAccessor.UserId(),
                GuestNicknames = poll.Guests.ToArray(),
                Proposals = poll.Proposals.ToArray(),
                Question = poll.Question
            });

            return createdPoll.ToGraphQL(
                () => new PollPayload(),
                (p, e) => p.Errors = e,
                p => new Poll
                {
                    PollId = p.PollId,
                    Question = p.Question
                },
                (payload, poll) => payload.Poll = poll);
        }

        public async Task<VotePayload> Vote(
            VoteInput vote,
            [Service] IPollService pollService,
            [Service] IPollRepository pollRepository,
            [Service] PollContext pollContext,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            int userId = httpContextAccessor.UserId();
            Result result = await pollService.Answer(pollContext, pollRepository, vote.PollId, userId, vote.ProposalId);
            return result.ToGraphQL(() => new VotePayload(), (p, e) => p.Errors = e);
        }
    }

    public class PollMutationRootType : ObjectType<PollMutationRoot>
    {
        protected override void Configure(IObjectTypeDescriptor<PollMutationRoot> descriptor)
        {
            descriptor.Field(x => x.CreatePoll(default, default, default, default, default, default))
                .Type<NonNullType<PollPayloadType>>()
                .Argument("poll", d =>
                {
                    d.Type<NonNullType<PollInputType>>();
                });

            descriptor.Field(x => x.Vote(default, default, default, default, default))
                .Type<NonNullType<VotePayloadType>>()
                .Argument("vote", d =>
                {
                    d.Type<NonNullType<VoteInputType>>();
                });
        }
    }
}