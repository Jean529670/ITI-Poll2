using System;
using System.Threading.Tasks;
using FluentAssertions;
using ITI.Poll.Model;
using ITI.Poll.Tests;
using NUnit.Framework;

namespace ITI.Poll.Infrastructure.Tests.Integration
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [Test]
        public async Task create_user()
        {
            using(PollContext pollContext = TestHelpers.CreatePollContext())
            {
                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                UserRepository sut = new UserRepository(pollContextAccessor);
                string email = $"{Guid.NewGuid()}@test.org";
                string nickname = $"Test-{Guid.NewGuid()}";
                Result<User> user = User.Create(email, nickname, "test-hash");

                Result creationStatus = await sut.Create(user.Value);

                creationStatus.IsSuccess.Should().BeTrue();
                Result<User> foundUser = await sut.FindByEmail(email);
                foundUser.IsSuccess.Should().BeTrue();
                foundUser.Value.Should().BeEquivalentTo(user.Value);

                PollRepository pollRepository = new PollRepository(pollContextAccessor);
                await TestHelpers.UserService.DeleteUser(pollContext, sut, pollRepository, foundUser.Value.UserId);
            }
        }
    }
}