using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using ITI.Poll.Model;

namespace ITI.Poll.GraphQL.Types
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public async Task<IEnumerable<Types.Poll>> Polls(
            [Service] IPollService pollService,
            [Service] IPollRepository pollRepository)
        {
            Result<IEnumerable<Model.Poll>> polls = await pollService.FindUserPolls(pollRepository, UserId);
            if (!polls.IsSuccess) return Enumerable.Empty<Types.Poll>();

            return polls.Value.Select(p => new Types.Poll
            {
                PollId = p.PollId,
                Question = p.Question
            });
        }

        public async Task<IEnumerable<Types.Invitation>> Invitations(
            [Service] IPollService pollService,
            [Service] IPollRepository pollRepository)
        {
            Result<IEnumerable<Model.Poll>> polls = await pollService.FindUserInvitations(pollRepository, UserId);
            if (!polls.IsSuccess) return Enumerable.Empty<Types.Invitation>();

            return polls.Value.Select(p => new Types.Invitation
            {
                InvitationId = p.PollId,
                Question = p.Question
            });
        }
    }

    public class UserType : ObjectType<User>
    {
        protected override void Configure(IObjectTypeDescriptor<User> descriptor)
        {
            descriptor.Field(x => x.UserId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(x => x.Nickname)
                .Type<NonNullType<StringType>>();

            descriptor.Field(x => x.Polls(default, default))
                .Type<NonNullType<ListType<NonNullType<PollType>>>>();

            descriptor.Field(x => x.Invitations(default, default))
                .Type<NonNullType<ListType<NonNullType<InvitationType>>>>();
        }
    }
}