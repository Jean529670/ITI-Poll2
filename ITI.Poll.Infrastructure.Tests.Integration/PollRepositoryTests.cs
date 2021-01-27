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
        public async Task create_guest()
        {
            using (PollContext pollContext = TestHelpers.CreatePollContext())
            {
                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                PollRepository sut = new PollRepository(pollContextAccessor);

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
            using (PollContext pollContext = TestHelpers.CreatePollContext())
            {

                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                var pollRepository = new PollRepository(pollContextAccessor);
                var userRepository = new UserRepository(pollContextAccessor);


                string email = $"test-{Guid.NewGuid()}@test.fr";
                string nickname = $"Test-{Guid.NewGuid()}";

                Result<User> user = await TestHelpers.UserService.CreateUser(userRepository, email, nickname, "validpassword");
                Result<User> guest2 = await TestHelpers.UserService.CreateUser(userRepository, $"{email}-guest2", $"{nickname}-guest2", "validpassword");
                Result<User> guest = await TestHelpers.UserService.CreateUser(userRepository, $"{email}-guest", $"{nickname}-guest", "validpassword");
                var pollDto = new NewPollDto
                {
                    AuthorId = user.Value.UserId,
                    Question = "Test-Question ",
                    GuestNicknames = new[] { guest.Value.Nickname, guest2.Value.Nickname },
                    Proposals = new[] { "proposal1", "proposal2" },
                };
                var pollCreated = await TestHelpers.PollService.CreatePoll(pollContext, pollRepository, userRepository, pollDto);

                var remove_guest = await TestHelpers.PollService.DeleteGuest(pollRepository, guest2.Value.UserId, pollCreated.Value.PollId);
                remove_guest.IsSuccess.Should().BeTrue();
                await TestHelpers.PollService.DeletePoll(pollContext, pollRepository, pollCreated.Value.PollId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, user.Value.UserId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, guest.Value.UserId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, guest2.Value.UserId);
            }
        }

        [Test]
        public async Task guest_add_answer_to_proposal()
        {
            using (PollContext pollContext = TestHelpers.CreatePollContext())
            {
                PollContextAccessor pollContextAccessor = new PollContextAccessor(pollContext);
                var pollRepository = new PollRepository(pollContextAccessor);
                var userRepository = new UserRepository(pollContextAccessor);

                string email = $"test-{Guid.NewGuid()}@coucou.fr";
                string nickname = $"Test-{Guid.NewGuid()}";

                Result<User> user = await TestHelpers.UserService.CreateUser(userRepository, email, nickname, "validpassword");
                Result<User> guest = await TestHelpers.UserService.CreateUser(userRepository, $"{email}-guest", $"{nickname}-guest", "validpassword");

                var pollDto = new NewPollDto
                {
                    AuthorId = user.Value.UserId,
                    Question = "Test-Question ",
                    GuestNicknames = new[] { guest.Value.Nickname },
                    Proposals = new[] { "proposal1", "proposal2" },
                };
                var pollCreated = await TestHelpers.PollService.CreatePoll(pollContext, pollRepository, userRepository, pollDto);
                var addAnswer = await TestHelpers.PollService.Answer(pollContext, pollRepository, pollCreated.Value.PollId, guest.Value.UserId, pollCreated.Value.Proposals[0].ProposalId);
                addAnswer.IsSuccess.Should().BeTrue();
                await TestHelpers.PollService.DeletePoll(pollContext, pollRepository, pollCreated.Value.PollId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, user.Value.UserId);
                await TestHelpers.UserService.DeleteUser(pollContext, userRepository, pollRepository, guest.Value.UserId);
            }
        }
    }
}