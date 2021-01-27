using System.Collections.Generic;
using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Poll
    {
        public int PollId { get; set; }

        public string Question { get; set; }

        public IEnumerable<Answer> Answers { get; set; }

        public int GuestCount { get; set; }
    }

    public class PollType : ObjectType<Poll>
    {
        protected override void Configure(IObjectTypeDescriptor<Poll> descriptor)
        {
            descriptor.Field(x => x.PollId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.Question)
                .Type<NonNullType<StringType>>();

            descriptor.Field(x => x.Answers)
                .Type<NonNullType<ListType<NonNullType<AnswerType>>>>();

            descriptor.Field(x => x.GuestCount)
                .Type<NonNullType<IntType>>();
        }
    }
}