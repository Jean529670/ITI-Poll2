using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public interface IUserDeletedEventHandler
    {
        Task Handle(IUnitOfWork unitOfWork, IPollRepository pollRepository, int userId);
    }
}