CREATE TABLE [dbo].[TournamentCondition] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [FirstName]     BIT NOT NULL,
    [LastName]      BIT NOT NULL,
    [PlaystationID] BIT NOT NULL,
    [XboxGametag]   BIT NOT NULL,
    [EAAccount]     BIT NOT NULL,
    [SteamAccount]  BIT NOT NULL,
    [GarenaAccount] BIT NOT NULL,
    [ICQ]           BIT NOT NULL,
    [Skype]         BIT NOT NULL,
    [Vk]            BIT NOT NULL,
    CONSTRAINT [PK_TournamentCondition] PRIMARY KEY CLUSTERED ([ID] ASC)
);

