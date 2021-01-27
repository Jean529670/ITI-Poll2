using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class VotePayload
    {
        public IEnumerable<Error> Errors { get; set; }
    }

    public class VotePayloadType : ObjectType<VotePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<VotePayload> descriptor)
        {
            descriptor.Field(x => x.Errors)
                .Type<NonNullType<ListType<NonNullType<ErrorType>>>>();
        }
    }
}