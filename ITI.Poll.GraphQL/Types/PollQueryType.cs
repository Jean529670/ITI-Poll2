using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using ITI.Poll.Model;
using Microsoft.AspNetCore.Http;

namespace ITI.Poll.GraphQL.Types
{
    public class PollQuery
    {
        public async Task<User> Me(
            [Service] IUserService userService,
            [Service] IUserRepository userRepository,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            int userId = httpContextAccessor.UserId();
            Result<Model.User> user = await userService.FindById(userRepository, userId);
            return new User
            {
                Email = user.Value.Email,
                Nickname = user.Value.Nickname,
                UserId = user.Value.UserId
            };
        }

        public async Task<CheckNicknamePayload> CheckNickname(
            [Service] IUserService userService,
            [Service] IUserRepository userRepository,
            string nickname)
        {
            Result<Model.User> user = await userService.FindByNickname(userRepository, nickname);
            return user.ToGraphQL(
                () => new CheckNicknamePayload(),
                (p, e) => p.Errors = e,
                u => new Guest
                {
                    GuestId = u.UserId,
                    Nickname = u.Nickname
                },
                (p, g) => p.Guest = g);
        }

        public async Task<PollPayload> Poll(
            [Service] IPollService pollService,
            [Service] IPollRepository pollRepository,
            [Service] IHttpContextAccessor httpContextAccessor,
            int pollId)
        {
            int authorId = httpContextAccessor.UserId();
            Result<Model.Poll> poll = await pollService.FindByIdForAuthor(pollRepository, authorId, pollId);
            return poll.ToGraphQL(
                () => new PollPayload(),
                (p, e) => p.Errors = e,
                p => new Poll
                {
                    PollId = p.PollId,
                    Question = p.Question,
                    Answers = p.Proposals.Select(proposal => new Answer
                    {
                        AnswerId = proposal.ProposalId,
                        Text = proposal.Text,
                        VotesCount = proposal.Voters.Count
                    }),
                    GuestCount = p.Guests.Count
                },
                (p, poll) => p.Poll = poll);
        }

        public async Task<InvitationPayload> Invitation(
            [Service] IPollService pollService,
            [Service] IPollRepository pollRepository,
            [Service] IHttpContextAccessor httpContextAccessor,
            int invitationId)
        {
            int guestId = httpContextAccessor.UserId();
            Result<Model.Poll> invitation = await pollService.FindByIdForGuest(pollRepository, invitationId, guestId);
            return invitation.ToGraphQL(
                () => new InvitationPayload(),
                (p, e) => p.Errors = e,
                p => new Invitation
                {
                    InvitationId = p.PollId,
                    Question = p.Question,
                    Proposals = p.Proposals.Select(proposal => new Proposal
                    {
                        ProposalId = proposal.ProposalId,
                        Text = proposal.Text
                    })
                },
                (p, i) => p.Invitation = i);
        }
    }

    public class PollQueryType : ObjectType<PollQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<PollQuery> descriptor)
        {
            descriptor.Field(x => x.Me(default, default, default))
                .Type<NonNullType<UserType>>()
                .Authorize(Policy.IsAuthenticated);

            descriptor.Field(x => x.CheckNickname(default, default, default))
                .Type<NonNullType<CheckNicknamePayloadType>>()
                .Argument("nickname", d =>
                {
                    d.Type<NonNullType<StringType>>();
                })
                .Authorize(Policy.IsAuthenticated);

            descriptor.Field(x => x.Poll(default, default, default, default))
                .Type<NonNullType<PollPayloadType>>()
                .Argument("pollId", d =>
                {
                    d.Type<NonNullType<IdType>>();
                })
                .Authorize(Policy.IsAuthenticated);

            descriptor.Field(x => x.Invitation(default, default, default, default))
                .Type<NonNullType<InvitationPayloadType>>()
                .Argument("invitationId", d =>
                {
                    d.Type<NonNullType<IdType>>();
                })
                .Authorize(Policy.IsAuthenticated);
        }
    }
}