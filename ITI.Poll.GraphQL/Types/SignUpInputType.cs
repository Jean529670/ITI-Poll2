using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class SignUpInput
    {
        public string Email { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }
    }

    public class SignUpInputType : InputObjectType<SignUpInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SignUpInput> descriptor)
        {
            descriptor.Field(i => i.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(i => i.Nickname)
                .Type<NonNullType<StringType>>();

            descriptor.Field(i => i.Password)
                .Type<NonNullType<StringType>>();
        }
    }
}