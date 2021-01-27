using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ITI.Poll.Model.Tests.Unit
{
    [TestFixture]
    public class PollServiceTests
    {
        [Test]
        public async Task create_poll()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            
            IPollRepository pollRepository = Substitute.For<IPollRepository>();
            pollRepository.Create(Arg.Any<Poll>()).Returns(Task.FromResult(Result.CreateSuccess()));

            IUserRepository userRepository = Substitute.For<IUserRepository>();
            userRepository.Create(Arg.Any<User>()).Returns(Task.FromResult(Result.CreateSuccess()));
            User user = new User(1234, "toto@mail.com", "toto", "passwordhash", false);
            userRepository.FindById(Arg.Any<int>()).Returns(Task.FromResult(Result.CreateSuccess(user)));
            userRepository.FindByNickname(Arg.Any<string>()).Returns(Task.FromResult(Result.CreateSuccess(user)));

            NewPollDto newPollDto = new NewPollDto();
            newPollDto.AuthorId = 1234;
            newPollDto.Question = "Question?";
            newPollDto.GuestNicknames = new string[] { "Guest1" };
            newPollDto.Proposals = new string[] { "P1", "P2" };
            PollService sut = new PollService();

            Result<Poll> poll = await sut.CreatePoll(unitOfWork, pollRepository, userRepository, newPollDto);

            poll.IsSuccess.Should().BeTrue();
        }
    }
}