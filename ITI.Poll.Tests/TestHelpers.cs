using ITI.Poll.AspNetCore;
using ITI.Poll.Infrastructure;
using ITI.Poll.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITI.Poll.Tests
{
    public static class TestHelpers
    {
        public static PollContext CreatePollContext()
        {
            return new PollContext(DbContextOptions);
        }

        static DbContextOptions<PollContext> _dbContextOptions;
        public static DbContextOptions<PollContext> DbContextOptions
        {
            get
            {
                if (_dbContextOptions == null)
                {
                    _dbContextOptions = new DbContextOptionsBuilder<PollContext>()
                        .UseSqlServer(ConnectionString)
                        .Options;
                }

                return _dbContextOptions;
            }
        }

        static string _connectionString;
        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null) _connectionString = Configuration["ConnectionStrings:Poll"];
                return _connectionString;
            }
        }

        static IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
                }

                return _configuration;
            }
        }

        static UserService _userService;
        public static UserService UserService
        {
            get
            {
                if (_userService == null) _userService = new UserService(PasswordHasher, UserDeletedEventHandler);

                return _userService;
            }
        }

        static PasswordHasher _passwordHasher;
        public static PasswordHasher PasswordHasher
        {
            get
            {
                if (_passwordHasher == null) _passwordHasher = new PasswordHasher();
                return _passwordHasher;
            }
        }

        static UserDeletedEventHandler _userDeletedEventHandler;
        public static UserDeletedEventHandler UserDeletedEventHandler
        {
            get
            {
                if (_userDeletedEventHandler == null) _userDeletedEventHandler = new UserDeletedEventHandler(PollService);
                return _userDeletedEventHandler;
            }
        }

        static PollService _pollService;
        public static PollService PollService
        {
            get
            {
                if (_pollService == null) _pollService = new PollService();
                return _pollService;
            }
        }
    }
}
