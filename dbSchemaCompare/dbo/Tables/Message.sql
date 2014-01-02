CREATE TABLE [dbo].[Message] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [SenderID]   INT            NOT NULL,
    [ReceiverID] INT            NOT NULL,
    [GameID]     INT            NULL,
    [MatchID]    INT            NULL,
    [GroupID]    INT            NULL,
    [AddedDate]  DATETIME       NOT NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    [SubjectID]  INT            NOT NULL,
    [IsSend]     BIT            NOT NULL,
    [ReadedDate] DATETIME       NULL,
    [IsDeleted]  BIT            NOT NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Message_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[Game] ([ID]),
    CONSTRAINT [FK_Message_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Message_Match] FOREIGN KEY ([MatchID]) REFERENCES [dbo].[Match] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Message_Receiver] FOREIGN KEY ([ReceiverID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Message_Sender] FOREIGN KEY ([SenderID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Message_Subject] FOREIGN KEY ([SubjectID]) REFERENCES [dbo].[Subject] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

