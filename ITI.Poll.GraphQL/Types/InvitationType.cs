using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Invitation
    {
        public int InvitationId { get; set; }

        public string Question { get; set; }

        public IEnumerable<Proposal> Proposals { get; set; }
    }

    public class InvitationType : ObjectType<Invitation>
    {
        protected override void Configure(IObjectTypeDescriptor<Invitation> descriptor)
        {
            descriptor.Field(x => x.InvitationId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.Question)
                .Type<NonNullType<StringType>>();

            descriptor.Field(x => x.Proposals)
                .Type<NonNullType<ListType<NonNullType<ProposalType>>>>();
        }
    }
}