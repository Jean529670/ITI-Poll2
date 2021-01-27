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
                UserRepository userRepository = new UserRepository(pollContextAccessor);

                // We should create Author user for create poll associated with this user
                string emailAuthor = $"{Guid.NewGuid()}@test.org";
                string nicknameAuthor = $"Test-{Guid.NewGuid()}";
                Result<User> userAuthorCreated = await TestHelpers.UserService.CreateUser(userRepository, emailAuthor, nicknameAuthor, "hashpassword");

                // We should create Guest user for create poll associated with this user
                string emailGuest = $"{Guid.NewGuid()}@test.org";
                string nicknameGuest = $"Test-{Guid.NewGuid()}";
                Result<User> userGuestCreated = await TestHelpers.UserService.CreateUser(userRepository, emailGuest, nicknameGuest, "hashpassword");

                NewPollDto newPollDto = new NewPollDto();
                newPollDto.AuthorId = userAuthorCreated.Value.UserId;
                newPollDto.Question = "Question?";
                newPollDto.GuestNicknames = new string[]{ userGuestCreated.Value.Nickname };
                newPollDto.Proposals = new string[] { "P1", "P2" };

                Result<Poll> poll = await TestHelpers.PollService.CreatePoll(pollContext, pollRepository, userRepository, newPollDto);


                poll.IsSuccess.Should().BeTrue();
                Result<Poll> foundPoll = await TestHelpers.PollService.FindById(pollRepository, poll.Value.PollId);
                poll.Should().BeEquivalentTo(foundPoll);

                await TestHelpers.PollService.DeletePoll(pollContext, pollRepository, poll.Value.PollId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, userAuthorCreated.Value.UserId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, userGuestCreated.Value.UserId);
            }
        }
    }
}