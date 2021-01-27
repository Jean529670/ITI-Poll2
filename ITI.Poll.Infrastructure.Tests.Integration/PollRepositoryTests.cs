using FluentAssertions;
using ITI.Poll.Model;
using ITI.Poll.Tests;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ITI.Poll.Infrastructure.Tests.Integration
{
    [TestFixture]
    public class PollRepositoryTests
    {
        [Test]
        public async Task create_poll()
        {
            using (PollContext pollContext = TestHelpers.CreatePollContext())
            {
                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                PollRepository sut = new PollRepository(pollContextAccessor);

                // We should create user for create poll associated with this user
                UserRepository userRepository = new UserRepository(pollContextAccessor);
                string email = $"{Guid.NewGuid()}@test.org";
                string nickname = $"Test-{Guid.NewGuid()}";
                User user = new User(0, email, nickname, "hash", false);
                Result userCreated = await userRepository.Create(user);

                Model.Poll poll = new Model.Poll(0, user.UserId, "Question?", false);
                
                Result creationStatus = await sut.Create(poll);

                userCreated.IsSuccess.Should().BeTrue();
                creationStatus.IsSuccess.Should().BeTrue();
                Result<Model.Poll> foundPoll = await sut.FindById(poll.PollId);
                foundPoll.IsSuccess.Should().BeTrue();
                foundPoll.Value.AuthorId.Should().Be(poll.AuthorId);
                foundPoll.Value.PollId.Should().Be(poll.PollId);
                foundPoll.Value.Question.Should().Be(poll.Question);
                foundPoll.Value.IsDeleted.Should().BeFalse();
            }
        }
    }
}