using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class InvitationPayload
    {
        public IEnumerable<Error> Errors { get; set; }

        public Invitation Invitation { get; set; }
    }

    public class InvitationPayloadType : ObjectType<InvitationPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<InvitationPayload> descriptor)
        {
            descriptor.Field(x => x.Errors)
                .Type<NonNullType<ListType<NonNullType<ErrorType>>>>();

            descriptor.Field(x => x.Invitation)
                .Type<InvitationType>();
        }
    }
}