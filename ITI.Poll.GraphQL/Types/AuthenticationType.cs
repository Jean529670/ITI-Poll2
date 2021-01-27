using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Authentication
    {
        public User User { get; set; }

        public string AccessToken { get; set; }
    }

    public class AuthenticationType : ObjectType<Authentication>
    {
        protected override void Configure(IObjectTypeDescriptor<Authentication> descriptor)
        {
            descriptor.Field(x => x.User)
                .Type<NonNullType<UserType>>();

            descriptor.Field(x => x.AccessToken)
                .Type<NonNullType<StringType>>();
        }
    }
}