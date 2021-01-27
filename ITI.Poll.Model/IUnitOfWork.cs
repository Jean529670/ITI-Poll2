using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
    }
}