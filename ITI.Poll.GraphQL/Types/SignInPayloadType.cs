using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class SignInPayload
    {
        public IEnumerable<Error> Errors { get; set; }

        public Authentication Authentication { get; set; }
    }

    public class SignInPayloadType : ObjectType<SignInPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<SignInPayload> descriptor)
        {
            descriptor.Field(x => x.Errors)
                .Type<NonNullType<ListType<NonNullType<ErrorType>>>>();

            descriptor.Field(x => x.Authentication)
                .Type<AuthenticationType>();
        }
    }
}