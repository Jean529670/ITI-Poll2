using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public string Text { get; set; }

        public int VotesCount { get; set; }
    }

    public class AnswerType : ObjectType<Answer>
    {
        protected override void Configure(IObjectTypeDescriptor<Answer> descriptor)
        {
            descriptor.Field(x => x.AnswerId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.Text)
                .Type<NonNullType<StringType>>();

            descriptor.Field(x => x.VotesCount)
                .Type<NonNullType<IntType>>();
        }
    }
}