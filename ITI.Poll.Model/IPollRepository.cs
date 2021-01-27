using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public interface IPollRepository
    {
        Task<Result> Create(Poll poll);

        Task<Proposal> GetNoProposal();
        
        Task<Result<Poll>> FindById(int pollId);
        
        void Remove(IEnumerable<object> toRemove);
        
        Task<Result<IEnumerable<Poll>>> FindUserPolls(int userId);
        
        Task<Result<IEnumerable<Poll>>> FindUserInvitations(int userId);
    }
}