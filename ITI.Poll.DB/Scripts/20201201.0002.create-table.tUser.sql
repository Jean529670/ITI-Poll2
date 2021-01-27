create table poll.tUser
(
    UserId       int identity(16, 1),
    Email        nvarchar(64) collate Latin1_General_100_CI_AI not null,
    Nickname     nvarchar(64) collate Latin1_General_100_CI_AI not null,
    PasswordHash varchar(256) collate Latin1_General_100_BIN2 not null,
    IsDeleted    bit not null,

    constraint PK_poll_tUser primary key(UserId),
    constraint UK_poll_tUser_Email unique(Email),
    constraint UK_poll_tUser_Nickname unique(Nickname)
);

set identity_insert poll.tUser on;
insert into poll.tUser(UserId, Email, Nickname, PasswordHash, IsDeleted)
                values(0,      N'',   N'',      '',           0);
set identity_insert poll.tUser off;