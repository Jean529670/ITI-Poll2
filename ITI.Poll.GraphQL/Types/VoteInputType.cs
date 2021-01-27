using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class VoteInput
    {
        public int PollId { get; set; }

        public int ProposalId { get; set; }
    }

    public class VoteInputType : InputObjectType<VoteInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<VoteInput> descriptor)
        {
            descriptor.Field(x => x.PollId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.ProposalId)
                .Type<NonNullType<IdType>>();
        }
    }
}