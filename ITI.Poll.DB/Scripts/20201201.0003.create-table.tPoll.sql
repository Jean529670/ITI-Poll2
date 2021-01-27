create table poll.tPoll
(
    PollId       int identity(16, 1),
    AuthorId     int not null,
    Question     nvarchar(64) collate Latin1_General_100_CI_AI not null,
    IsDeleted    bit not null,

    constraint PK_poll_tPoll primary key(PollId),
    constraint FK_poll_tPoll_AuthorId foreign key(AuthorId) references poll.tUser(UserId),
    constraint CK_poll_tPoll_Question check(PollId < 16 or Question <> N'')
);

set identity_insert poll.tPoll on;
insert into poll.tPoll(PollId, AuthorId, Question, IsDeleted)
                values(0,      0,        N'',      0);
set identity_insert poll.tPoll off;