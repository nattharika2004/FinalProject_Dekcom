CREATE TABLE [dbo].[email_replies] (
    [ReplyID]      INT      IDENTITY (1, 1) NOT NULL,
    [EmailID]      INT      NOT NULL,
    [ReplyMessage] TEXT     NOT NULL,
    [ReplyDate]    DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([ReplyID] ASC),
    FOREIGN KEY ([EmailID]) REFERENCES [dbo].[Emails] ([Id])
);