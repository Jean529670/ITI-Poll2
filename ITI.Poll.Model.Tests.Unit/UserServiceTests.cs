using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace ITI.Poll.Model.Tests.Unit
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public async Task create_user()
        {
            IPasswordHasher passwordHasher = Substitute.For<IPasswordHasher>();
            passwordHasher.HashPassword(Arg.Any<string>()).Returns("hash");
            IUserDeletedEventHandler userDeletedEventHandler = Substitute.For<IUserDeletedEventHandler>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            userRepository.Create(Arg.Any<User>()).Returns(Task.FromResult(Result.CreateSuccess()));
            UserService sut = new UserService(passwordHasher, userDeletedEventHandler);
            
            Result<User> user = await sut.CreateUser(userRepository, "test@email.org", "Test", "validpassword");

            Result<User> expected = Result.CreateSuccess(new User(0, "test@email.org", "Test", "hash", false));
            user.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task create_user_with_existing_email()
        {
            IPasswordHasher passwordHasherStub = Substitute.For<IPasswordHasher>();
            passwordHasherStub.HashPassword(Arg.Any<string>()).Returns("hash");
            IUserDeletedEventHandler userDeletedEventHandlerStub = Substitute.For<IUserDeletedEventHandler>();
            IUserRepository userRepositoryStub = Substitute.For<IUserRepository>();
            userRepositoryStub.Create(Arg.Any<User>()).Returns(Task.FromResult(Result.CreateError("UserAlreadyExists", "The user already exsists.")));
            UserService sut = new UserService(passwordHasherStub, userDeletedEventHandlerStub);

            Result<User> user = await sut.CreateUser(userRepositoryStub, "test@email.org", "Test", "longpassword");

            user.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task CreateUser_uses_IUserRepository_correctly()
        {
            IPasswordHasher passwordHasherStub = Substitute.For<IPasswordHasher>();
            passwordHasherStub.HashPassword(Arg.Any<string>()).Returns("hash");
            IUserDeletedEventHandler userDeletedEventHandlerStub = Substitute.For<IUserDeletedEventHandler>();
            IUserRepository userRepositoryMock = Substitute.For<IUserRepository>();
            userRepositoryMock.Create(Arg.Any<User>()).Returns(Task.FromResult(Result.CreateSuccess()));
            UserService sut = new UserService(passwordHasherStub, userDeletedEventHandlerStub);

            await sut.CreateUser(userRepositoryMock, "test@email.org", "Test", "validpassword");

            await userRepositoryMock.ReceivedWithAnyArgs(1).Create(Arg.Any<User>());
        }

        [Test]
        public async Task CreateUser_uses_IPasswordHasher_correctly()
        {
            IPasswordHasher passwordHasherMock = Substitute.For<IPasswordHasher>();
            passwordHasherMock.HashPassword(Arg.Any<string>()).Returns("hash");
            IUserDeletedEventHandler userDeletedEventHandlerStub = Substitute.For<IUserDeletedEventHandler>();
            IUserRepository userRepositoryStub = Substitute.For<IUserRepository>();
            userRepositoryStub.Create(Arg.Any<User>()).Returns(Task.FromResult(Result.CreateSuccess()));
            UserService sut = new UserService(passwordHasherMock, userDeletedEventHandlerStub);

            await sut.CreateUser(userRepositoryStub, "test@email.org", "Test", "validpassword");

            passwordHasherMock.Received(1).HashPassword("validpassword");
        }

        [Test]
        public async Task authentication_should_succeed_with_correct_email_and_password_hash()
        {
            IPasswordHasher passwordHasher = Substitute.For<IPasswordHasher>();
            passwordHasher.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            IUserDeletedEventHandler userDeletedEventHandler = Substitute.For<IUserDeletedEventHandler>();
            IUserRepository userRepository = Substitute.For<IUserRepository>();
            User user = new User(1234, "test@test.fr", "Test", "hash", false);
            userRepository.FindByEmail(Arg.Any<string>()).Returns(Task.FromResult(Result.CreateSuccess(user)));
            UserService sut = new UserService(passwordHasher, userDeletedEventHandler);

            Result<User> result = await sut.Authenticate(userRepository, "test@test.fr", "validpassword");

            result.Should().BeEquivalentTo(Result.CreateSuccess(user));
        }
    }
}