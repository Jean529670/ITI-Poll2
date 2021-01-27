using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public interface IUserService
    {
        Task<Result<User>> CreateUser(IUserRepository userRepository, string email, string nickname, string password);
        
        Task<Result> DeleteUser(IUnitOfWork unitOfWork, IUserRepository userRepository, IPollRepository pollRepository, int userId);
        
        Task<Result<User>> Authenticate(IUserRepository userRepository, string email, string password);

        Task<Result<User>> FindById(IUserRepository userRepository, int userId);
        
        Task<Result<User>> FindByNickname(IUserRepository userRepository, string nickname);
    }
}