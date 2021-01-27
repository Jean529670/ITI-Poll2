using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ITI.Poll.Model;

namespace ITI.Poll.Specs.Drivers
{
    public sealed class VoteDriver
    {
        List<Guest> _guests = new List<Guest>();
        Model.Poll _poll;
        Model.Guest _voter;
        Model.Proposal _vote;
        Result _answerResult;

        public void CreatePoll()
        {
            Model.Poll dummyPoll = new Model.Poll(3712, 1, "Question?", false);
            Model.Proposal dummyProposal = dummyPoll.AddProposal(string.Empty);
            _poll = new Model.Poll(3712, 1, "Question?", false);

            _guests.Add(_poll.AddGuest(2, dummyProposal));
            _guests.Add(_poll.AddGuest(3, dummyProposal));
            _guests.Add(_poll.AddGuest(4, dummyProposal));

            {
                Model.Proposal p = _poll.AddProposal("Proposal 1");
                p.ProposalId = 1;
            }

            {
                Model.Proposal p = _poll.AddProposal("Proposal 2");
                p.ProposalId = 2;
            }

            {
                Model.Proposal p = _poll.AddProposal("Proposal 3");
                p.ProposalId = 3;
            }
        }

        public void Vote(int userId, int voteIdx)
        {
            _voter = _guests.Single(g => g.UserId == userId);
            _vote = _poll.Proposals.Skip(voteIdx - 1).First();
            _answerResult = _poll.Answer(userId, _vote.ProposalId);
        }

        public void CheckVote(int voteIdx)
        {
            _answerResult.IsSuccess.Should().BeTrue();
            Model.Proposal vote = _poll.Proposals.Skip(voteIdx - 1).First();
            _voter.Vote.Should().BeSameAs(vote);
        }

        public void CheckVoter(int userId)
        {
            _vote.Voters.Should().Contain(g => g.UserId == userId);
        }
    }
}
