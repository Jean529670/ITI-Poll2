using FluentAssertions;
using ITI.Poll.Infrastructure;
using ITI.Poll.Tests;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ITI.Poll.Model.Tests.Integration
{
    [TestFixture]
    public class PollServiceTests
    {
        [Test]
        public async Task create_poll()
        {
            using (PollContext pollContext = TestHelpers.CreatePollContext())
            {
                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                PollRepository pollRepository = new PollRepository(pollContextAccessor);

                // We should create Author user for create poll associated with this user
                UserRepository userRepository = new UserRepository(pollContextAccessor);
                string emailAuthor = $"{Guid.NewGuid()}@test.org";
                string nicknameAuthor = $"Test-{Guid.NewGuid()}";
                User userAuthor = new User(0, emailAuthor, nicknameAuthor, "hash", false);
                Result userAuthorCreated = await userRepository.Create(userAuthor);

                // We should create Author user for create poll associated with this user
                string emailGuest = $"{Guid.NewGuid()}@test.org";
                string nicknameGuest = $"Test-{Guid.NewGuid()}";
                User userGuest = new User(0, emailGuest, nicknameGuest, "hash", false);
                Result userGuestCreated = await userRepository.Create(userGuest);

                NewPollDto newPollDto = new NewPollDto();
                newPollDto.AuthorId = userAuthor.UserId;
                newPollDto.Question = "Question?";
                newPollDto.GuestNicknames = new string[]{ userGuest.Nickname };
                newPollDto.Proposals = new string[] { "P1", "P2" };

                Result<Poll> poll = await TestHelpers.PollService.CreatePoll(pollContext, pollRepository, userRepository, newPollDto);


                poll.IsSuccess.Should().BeTrue();
                Result<Poll> foundPoll = await TestHelpers.PollService.FindById(pollRepository, poll.Value.PollId);
                poll.Should().BeEquivalentTo(foundPoll);
            }
        }
    }
}