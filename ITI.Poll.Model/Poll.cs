using System;
using System.Collections.Generic;
using System.Linq;

namespace ITI.Poll.Model
{
    public sealed class Poll
    {
        static readonly string UnknownGuestMsg = "Unknown guest.";
        static readonly string AlreadyAnsweredMsg = "This user has already answered.";
        static readonly string UnknownProposalMsg = "Unknown proposal.";

        readonly List<Proposal> _proposals;
        readonly List<Guest> _guests;

        public Poll(int pollId, int authorId, string question, bool isDeleted)
        {
            if (string.IsNullOrWhiteSpace(question)) throw new ArgumentException("The question must be not null not white space.", nameof(question));

            _proposals = new List<Proposal>();
            _guests = new List<Guest>();

            PollId = pollId;
            AuthorId = authorId;
            Question = question;
            IsDeleted = isDeleted;
        }

        public int PollId { get; set; }

        public int AuthorId { get; private set; }

        public string Question { get; private set; }

        public IReadOnlyList<Proposal> Proposals => _proposals;

        public IReadOnlyList<Guest> Guests => _guests;

        public bool IsDeleted { get; private set; }

        public Proposal AddProposal(string text)
        {
            Proposal proposal = Proposal.Create(0, text, this);
            _proposals.Add(proposal);
            return proposal;
        }

        public Guest AddGuest(int userId, Proposal vote)
        {
            Guest guest = Guest.Create(userId, this, vote);
            _guests.Add(guest);
            return guest;
        }

        public IEnumerable<object> Delete()
        {
            List<object> removed = new List<object>();

            IsDeleted = true;
            AuthorId = 0;
            Question = $"Deleted-{Guid.NewGuid()}";

            foreach(Proposal proposal in _proposals) 
            {
                proposal.OnDelete();
                removed.Add(proposal);
            }
            _proposals.Clear();

            foreach(Guest guest in _guests) 
            {
                guest.OnDelete();
                removed.Add(guest);
            }
            _guests.Clear();

            return removed;
        }

        public Result Answer(int guestId, int proposalId)
        {
            Guest guest = _guests.SingleOrDefault(g => g.UserId == guestId);
            if (guest == null) return Result.CreateError(Errors.UnknownGuest, UnknownGuestMsg);
            if (guest.Vote.ProposalId != 0) return Result.CreateError(Errors.AlreadyAnswered, AlreadyAnsweredMsg);

            Proposal proposal = _proposals.SingleOrDefault(p => p.ProposalId == proposalId);
            if (proposal == null) return Result.CreateError(Errors.UnknownProposal, UnknownProposalMsg);

            guest.Answer(proposal);
            return Result.CreateSuccess();
        }

        public Result RemoveGuest(int guestId)
        {
            Guest guest = _guests.SingleOrDefault(g => g.UserId == guestId);
            if (guest == null) return Result.CreateError(Errors.UnknownGuest, UnknownGuestMsg);
            _guests.Remove(guest);
            return Result.CreateSuccess();
        }
    }

    public sealed class Proposal
    {
        readonly List<Guest> _voters;

        Proposal(int proposalId, string text)
        {
            _voters = new List<Guest>();
            
            ProposalId = proposalId;
            Text = text;
        }

        public int ProposalId { get; set; }

        public string Text { get; private set; }

        public Poll Poll { get; private set; }

        public IReadOnlyCollection<Guest> Voters => _voters;

        internal static Proposal Create(int proposalId, string text, Poll poll)
            => new Proposal(proposalId, text) { Poll = poll };

        internal void OnDelete()
        {
            _voters.Clear();
            Text = string.Empty;
            Poll = null;
        }

        internal void OnChoose(Guest guest)
        {
            _voters.Add(guest);
        }
    }

    public sealed class Guest
    {
        int _pollId;
        int _userId;

        Guest(int userId, int pollId)
        {
            _userId = userId;
            _pollId = pollId;
        }

        public int PollId => _pollId;

        public Poll Poll { get; private set; }

        public Proposal Vote { get; private set; }

        public int UserId => _userId;

        internal static Guest Create(int userId, Poll poll, Proposal vote)
            => new Guest(userId, poll.PollId) { Poll = poll, Vote = vote };

        internal void OnDelete()
        {
            Poll = null;
            Vote = null;
        }

        internal void Answer(Proposal proposal)
        {
            Vote = proposal;
            proposal.OnChoose(this);
        }
    }
}