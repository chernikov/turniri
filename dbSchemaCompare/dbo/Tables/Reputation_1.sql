CREATE TABLE [dbo].[Reputation] (
    [ID]             INT IDENTITY (1, 1) NOT NULL,
    [SenderID]       INT NOT NULL,
    [ReceiverID]     INT NOT NULL,
    [ReputationType] INT NOT NULL,
    [Mark]           INT NOT NULL,
    CONSTRAINT [PK_Reputation] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Reputation_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Reputation_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID])
);

