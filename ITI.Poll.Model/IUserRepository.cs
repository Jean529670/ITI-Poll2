using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public interface IUserRepository
    {
        Task<Result> Create(User user);

        Task<Result<User>> FindById(int userId);

        Task<Result<User>> FindByEmail(string email);
        
        Task<Result<User>> FindByNickname(string nickname);
    }
}