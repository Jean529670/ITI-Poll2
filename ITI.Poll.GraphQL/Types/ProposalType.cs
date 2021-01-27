using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Proposal
    {
        public int ProposalId { get; set; }

        public string Text { get; set; }
    }

    public class ProposalType : ObjectType<Proposal>
    {
        protected override void Configure(IObjectTypeDescriptor<Proposal> descriptor)
        {
            descriptor.Field(x => x.ProposalId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.Text)
                .Type<NonNullType<StringType>>();
        }
    }
}