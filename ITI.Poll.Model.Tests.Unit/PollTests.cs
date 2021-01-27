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

        static Poll CreateSut() => new Poll(1234, 4321, "Question?", false);
    }
}