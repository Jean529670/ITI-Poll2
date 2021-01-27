using HotChocolate.Types;

namespace ITI.Poll.GraphQL.Types
{
    public class PollMutation
    {
        public UserMutation User() => new UserMutation();

        public PollMutationRoot Poll() => new PollMutationRoot();
    }

    public class PollMutationType : ObjectType<PollMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<PollMutation> descriptor)
        {
            descriptor.Field(p => p.User())
                .Type<NonNullType<UserMutationType>>();

            descriptor.Field(p => p.Poll())
                .Type<NonNullType<PollMutationRootType>>()
                .Authorize(Policy.IsAuthenticated);
        }
    }
}