using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class Guest
    {
        public int GuestId { get; set; }

        public string Nickname { get; set; }
    }

    public class GuestType : ObjectType<Guest>
    {
        protected override void Configure(IObjectTypeDescriptor<Guest> descriptor)
        {
            descriptor.Field(x => x.GuestId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(x => x.Nickname)
                .Type<NonNullType<StringType>>();
        }
    }
}