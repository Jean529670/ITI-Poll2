create table poll.tProposal
(
    ProposalId int identity(16, 1),
    [Text]     nvarchar(128) collate Latin1_General_100_CI_AI not null,
    PollId     int not null,

    constraint PK_poll_tProposal primary key(ProposalId),
    constraint CK_poll_tProposal_Text check(ProposalId < 16 or [Text] <> N''),
    constraint FK_poll_tProposal_PollId foreign key(PollId) references poll.tPoll(PollId)
);

set identity_insert poll.tProposal on;
insert into poll.tProposal(ProposalId, [Text], PollId)
                    values(0,          N'',    0);
set identity_insert poll.tProposal off;