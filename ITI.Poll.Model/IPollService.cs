using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Poll.Model
{
    public interface IPollService
    {
        Task<Result<Poll>> CreatePoll(IUnitOfWork unitOfWork, IPollRepository pollRepository, IUserRepository userRepository, NewPollDto newPollInfo);
        
        Task<Result<Poll>> FindById(IPollRepository pollRepository, int pollId);

        Task<Result> DeletePoll(IUnitOfWork unitOfWork, IPollRepository pollRepository, int pollId);
        
        Task<Result> Answer(IUnitOfWork unitOfWork, IPollRepository pollRepository, int pollId, int userId, int proposalId);
        
        Task<Result<IEnumerable<Poll>>> FindUserPolls(IPollRepository pollRepository, int userId);
        
        Task<Result<Poll>> FindByIdForAuthor(IPollRepository pollRepository, int authorId, int pollId);
        
        Task<Result<IEnumerable<Poll>>> FindUserInvitations(IPollRepository pollRepository, int userId);

        Task<Result<Poll>> FindByIdForGuest(IPollRepository pollRepository, int pollId, int guestId);

        Task<Result> DeleteGuest(IPollRepository pollRepository, int guestId, int pollId);
    }

    public sealed class NewPollDto
    {
        public int AuthorId { get; set; }

        public string Question { get; set; }

        public string[] Proposals { get; set; }

        public string[] GuestNicknames { get; set; }
    }
}