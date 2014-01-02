CREATE TABLE [dbo].[Grade] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [SenderID]   INT NOT NULL,
    [ReceiverID] INT NOT NULL,
    [Grade]      INT NOT NULL,
    CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Grade_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Grade_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID])
);

