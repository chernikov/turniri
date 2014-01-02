CREATE TABLE [dbo].[Friendship] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [SenderID]   INT NOT NULL,
    [ReceiverID] INT NOT NULL,
    [Approved]   BIT NOT NULL,
    CONSTRAINT [PK_Friendship] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Friendship_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Friendship_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID])
);

