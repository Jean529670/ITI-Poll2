using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class SignInInput
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class SignInInputType : InputObjectType<SignInInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SignInInput> descriptor)
        {
            descriptor.Field(i => i.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(i => i.Password)
                .Type<NonNullType<StringType>>();
        }
    }
}