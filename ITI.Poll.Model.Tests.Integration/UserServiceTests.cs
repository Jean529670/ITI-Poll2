using System;
using System.Threading.Tasks;
using FluentAssertions;
using ITI.Poll.Infrastructure;
using ITI.Poll.Tests;
using NUnit.Framework;

namespace ITI.Poll.Model.Tests.Integration
{
    public class UserServiceTests
    {
        [Test]
        public async Task create_user()
        {
            using(PollContext pollContext = TestHelpers.CreatePollContext())
            {
                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                UserRepository userRepository = new UserRepository(pollContextAccessor);
                string email = $"test-{Guid.NewGuid()}@test.fr";
                string nickname = $"Test-{Guid.NewGuid()}";

                Result<User> user = await TestHelpers.UserService.CreateUser(userRepository, email, nickname, "validpassword");

                user.IsSuccess.Should().BeTrue();
                user.Value.Email.Should().Be(email);
                user.Value.Nickname.Should().Be(nickname);

                user = await TestHelpers.UserService.FindByNickname(userRepository, nickname);
                user.IsSuccess.Should().BeTrue();

                PollRepository pollRepository = new PollRepository(pollContextAccessor);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, user.Value.UserId);
            }
        }
    }
}