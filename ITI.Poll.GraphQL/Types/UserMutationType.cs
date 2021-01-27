using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using ITI.Poll.GraphQL.Services;
using ITI.Poll.Model;

namespace ITI.Poll.GraphQL.Types
{
    public class UserMutation
    {
        public async Task<SignInPayload> SignUp(
            [Service] IUserService userService,
            [Service] IUserRepository userRepository,
            [Service] TokenService tokenService,
            SignUpInput login)
        {
            Result<Model.User> user = await userService.CreateUser(userRepository, login.Email, login.Nickname, login.Password);
            return ToSignInPayload(tokenService, user);
        }

        public async Task<SignInPayload> SignIn(
            [Service] IUserService userService,
            [Service] IUserRepository userRepository,
            [Service] TokenService tokenService,
            SignInInput login)
        {
            Result<Model.User> user = await userService.Authenticate(userRepository, login.Email, login.Password);
            return ToSignInPayload(tokenService, user);
        }

        public static SignInPayload ToSignInPayload(TokenService tokenService, Result<Model.User> user)
        {
            return user.ToGraphQL(
                () => new SignInPayload(),
                (p, e) => p.Errors = e,
                u => new Authentication
                {
                    AccessToken = tokenService.GenerateToken(user.Value.UserId.ToString()),
                    User = new User
                    {
                        Email = user.Value.Email,
                        Nickname = user.Value.Nickname,
                        UserId = user.Value.UserId
                    }
                },
                (p, a) => p.Authentication = a);
        }
    }

    public class UserMutationType : ObjectType<UserMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<UserMutation> descriptor)
        {
            descriptor.Field(x => x.SignUp(default, default, default, default))
                .Type<NonNullType<SignInPayloadType>>()
                .Argument("login", d =>
                {
                    d.Type<NonNullType<SignUpInputType>>();
                });

            descriptor.Field(x => x.SignIn(default, default, default, default))
                .Type<NonNullType<SignInPayloadType>>()
                .Argument("login", d =>
                {
                    d.Type<NonNullType<SignInInputType>>();
                });
        }
    }
}