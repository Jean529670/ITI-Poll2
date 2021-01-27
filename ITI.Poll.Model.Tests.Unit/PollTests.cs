using System;
using FluentAssertions;
using NUnit.Framework;

namespace ITI.Poll.Model.Tests.Unit
{
    [TestFixture]
    public class PollTests
    {
        [Test]
        public void add_proposal_to_poll()
        {
            Poll sut = CreateSut();
            sut.AddProposal("proposal");
            sut.Proposals.Should().Contain(p => p.Text == "proposal");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public void initialize_poll_with_invalid_question_should_throw_ArgumentException(string invalidQuestion)
        {
            Action act = () => new Poll(1234, 4321, invalidQuestion, false);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void proposal_belongs_to_the_right_poll()
        {
            Poll sut = CreateSut();
            Proposal proposal = sut.AddProposal("Proposal");
            proposal.Poll.Should().BeSameAs(sut);
        }

        [Test]
        public void poll_contains_all_created_proposals()
        {
            Poll sut = CreateSut();

            Proposal p1 = sut.AddProposal("P1");
            Proposal p2 = sut.AddProposal("P2");
            Proposal p3 = sut.AddProposal("P3");

            sut.Proposals.Should().BeEquivalentTo(new[] { p2, p3, p1 });
        }

        [Test]
        public void add_guest_to_poll()
        {
            Poll sut = CreateSut();
            var proposal = sut.AddProposal("Proposal");
            sut.AddGuest(1234, proposal);

            sut.Guests.Should().HaveCount(1);
        }

        [Test]
        public void remove_guest_from_pool()
        {
            Poll sut = CreateSut();
            var proposal = sut.AddProposal("Proposal");
            sut.AddGuest(1234, proposal);

            sut.RemoveGuest(1234);

            sut.Guests.Should().HaveCount(0);
        }

        [Test]
        public void answer_with_unknow_guest_should_return_error_message()
        {
            Poll sut = CreateSut();
            var proposal = sut.AddProposal("Proposal");

            var result = sut.Answer(12345, proposal.ProposalId);

            result.ErrorMessage.Should().Be("Unknown guest.");
        }

        [Test]
        public void answer_with_unknow_proposal_should_return_error_message()
        {
            Poll sut = CreateSut();
            var proposal = sut.AddProposal("Proposal");
            sut.AddGuest(1234, proposal);

            var result = sut.Answer(1234, 12345);

            result.ErrorMessage.Should().Be("Unknown proposal.");
        }

        [Test]
        public void user_already_aswered_should_return_error_message()
        {
            Poll sut = CreateSut();
            var proposal = sut.AddProposal("Proposal");
            proposal.ProposalId = 1;
            sut.AddGuest(1234, proposal);

            sut.Answer(1234, proposal.ProposalId);

            var result = sut.Answer(1234, proposal.ProposalId);

            result.ErrorMessage.Should().Be("This user has already answered.");
        }

        static Poll CreateSut() => new Poll(1234, 4321, "Question?", false);
    }
}