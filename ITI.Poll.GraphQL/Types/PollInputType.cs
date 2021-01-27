using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class PollInput
    {
        public string Question { get; set; }

        public IEnumerable<string> Proposals { get; set; }

        public IEnumerable<string> Guests { get; set; }
    }

    public class PollInputType : InputObjectType<PollInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PollInput> descriptor)
        {
            descriptor.Field(x => x.Question)
                .Type<NonNullType<StringType>>();

            descriptor.Field(x => x.Proposals)
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();

            descriptor.Field(x => x.Guests)
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}