using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Error
    {
        public string Type { get; set; }

        public string Message { get; set; }
    }

    public class ErrorType : ObjectType<Error>
    {
        protected override void Configure(IObjectTypeDescriptor<Error> descriptor)
        {
            descriptor.Field(e => e.Type)
                .Type<NonNullType<StringType>>();

            descriptor.Field(e => e.Message)
                .Type<NonNullType<StringType>>();
        }
    }
}