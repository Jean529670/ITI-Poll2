namespace ITI.Poll.Model
{
    public static class Errors
    {
        public static readonly string InvalidEmail = "ITI.Poll.User.InvalidEmail";
        public static readonly string InvalidNickName = "ITI.Poll.User.InvalidNickName";
        public static readonly string EmptyPassword = "ITI.Poll.User.EmptyPassword";
        public static readonly string TooShortPassword = "ITI.Poll.User.TooShortPassword";
        public static readonly string UnknownGuest = "ITI.Poll.Polls.UnknownGuest";
        public static readonly string AlreadyAnswered = "ITI.Poll.Polls.AlreadyAnswered";
        public static readonly string UnknownProposal = "ITI.Poll.Polls.UnknownProposal";
        public static readonly string InvalidAuthorId = "ITI.Poll.Polls.InvalidAuthorId";
        public static readonly string NotEnoughGuests = "ITI.Poll.Polls.NotEnoughGuests";
        public static readonly string AuthorCannotBeGuest = "ITI.Poll.Polls.AuthorCannotBeGuest";
        public static readonly string EmptyQuestion = "ITI.Poll.Polls.EmptyQuestion";
        public static readonly string NotEnoughProposals = "ITI.Poll.Polls.NotEnoughProposals";
        public static readonly string EmptyProposal = "ITI.Poll.Polls.EmptyProposal";
        public static readonly string Forbidden = "ITI.Poll.Polls.Forbidden";
        public static readonly string AuthFailure = "ITI.Poll.Authentication.Failure";
    }
}