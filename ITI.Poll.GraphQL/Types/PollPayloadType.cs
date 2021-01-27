using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class PollPayload
    {
        public IEnumerable<Error> Errors { get; set; }

        public Poll Poll { get; set; }
    }

    public class PollPayloadType : ObjectType<PollPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<PollPayload> descriptor)
        {
            descriptor.Field(x => x.Errors)
                .Type<NonNullType<ListType<NonNullType<ErrorType>>>>();

            descriptor.Field(x => x.Poll)
                .Type<PollType>();
        }
    }
}