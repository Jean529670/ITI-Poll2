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
            //IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            //unitOfWork.

            //TestHelpers testHelpers = new 

            //IPollRepository pollRepository = Substitute.For<IPollRepository>();
            //pollRepository.Create(Arg.Any<Poll>()).Returns(Task.FromResult(Result.CreateSuccess()));

            //IUserRepository userRepository = Substitute.For<IUserRepository>();
            //userRepository.Create(Arg.Any<User>()).Returns(Task.FromResult(Result.CreateSuccess()));

            ////NewPollDto newPollDto = Substitute.For<NewPollDto>();
            //NewPollDto newPollDto = new NewPollDto();
            //newPollDto.AuthorId =0;
            //newPollDto.Question = "Question?";
            //PollService sut = new PollService();

            //Result<Poll> poll = await sut.CreatePoll(unitOfWork, pollRepository, userRepository, newPollDto);

            //Result<Poll> expected = Result.CreateSuccess(new Poll(0, 0, "Question?", false));
            //poll.Should().BeEquivalentTo(expected);
        }
    }
}