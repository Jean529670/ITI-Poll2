using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class CheckNicknamePayload
    {
        public IEnumerable<Error> Errors { get; set; }

        public Guest Guest { get; set; }
    }

    public class CheckNicknamePayloadType : ObjectType<CheckNicknamePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CheckNicknamePayload> descriptor)
        {
            descriptor.Field(x => x.Errors)
                .Type<NonNullType<ListType<NonNullType<ErrorType>>>>();

            descriptor.Field(x => x.Guest)
                .Type<GuestType>();
        }
    }
}