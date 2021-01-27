create table poll.tGuest
(
    PollId int not null,
    UserId int not null,
    VoteId int not null,

    constraint PK_poll_tGuest primary key(PollId, UserId),
    constraint FK_poll_tGuest_PollId foreign key(PollId) references poll.tPoll(PollId),
    constraint FK_poll_tGuest_UserId foreign key(UserId) references poll.tUser(UserId),
    constraint FK_poll_tGuest_VoteId foreign key(VoteId) references poll.tProposal(ProposalId)
);