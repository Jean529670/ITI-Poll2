using ITI.Poll.Specs.Drivers;
using TechTalk.SpecFlow;

namespace ITI.Poll.Specs.Steps
{
    [Binding]
    public class VoteSteps
    {
        private readonly VoteDriver _driver;

        public VoteSteps(VoteDriver driver)
        {
            _driver = driver;
        }

        [Given(@"a poll with some proposals")]
        public void CreatePoll()
        {
            _driver.CreatePoll();
        }

        [When(@"user with (\d+) vote for the proposal (\d+)")]
        public void Vote(int userId, int proposalIndex)
        {
            _driver.Vote(userId, proposalIndex);
        }

        [Then(@"his vote should be the proposal (\d+)")]
        public void CheckVote(int expectedProposalIndex)
        {
            _driver.CheckVote(expectedProposalIndex);
        }

        [Then(@"the voter should be the user with (\d+)")]
        public void CheckVoter(int userId)
        {
            _driver.CheckVoter(userId);
        }
    }

    public sealed class TestUserData
    {
        public int Id { get; set; }

        public string Nickname { get; set; }

        public string Email { get; set; }
    }
}
