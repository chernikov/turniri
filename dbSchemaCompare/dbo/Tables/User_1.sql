﻿CREATE TABLE [dbo].[User] (
    [ID]                       INT            IDENTITY (1, 1) NOT NULL,
    [Login]                    NVARCHAR (50)  NOT NULL,
    [Email]                    NVARCHAR (150) NULL,
    [VerifiedEmail]            BIT            NOT NULL,
    [Password]                 NVARCHAR (50)  NOT NULL,
    [AddedDate]                DATETIME       NOT NULL,
    [ActivatedDate]            DATETIME       NULL,
    [ActivatedLink]            NVARCHAR (50)  NOT NULL,
    [LastVisitDate]            DATETIME       NOT NULL,
    [AvatarPath173]            NVARCHAR (150) NULL,
    [AvatarPath96]             NVARCHAR (150) NULL,
    [AvatarPath84]             NVARCHAR (150) NULL,
    [AvatarPath57]             NVARCHAR (150) NULL,
    [AvatarPath26]             NVARCHAR (150) NULL,
    [AvatarPath18]             NVARCHAR (150) NULL,
    [FirstName]                NVARCHAR (500) NULL,
    [LastName]                 NVARCHAR (500) NULL,
    [Country]                  NVARCHAR (500) NULL,
    [City]                     NVARCHAR (500) NULL,
    [Address]                  NVARCHAR (500) NULL,
    [Phone]                    NVARCHAR (50)  NULL,
    [Birthdate]                DATETIME       NULL,
    [PlaystationID]            NVARCHAR (50)  NULL,
    [XboxGametag]              NVARCHAR (50)  NULL,
    [EAAccount]                NVARCHAR (50)  NULL,
    [SteamAccount]             NVARCHAR (50)  NULL,
    [GarenaAccount]            NVARCHAR (50)  NULL,
    [ICQ]                      NVARCHAR (50)  NULL,
    [Skype]                    NVARCHAR (50)  NULL,
    [Vk]                       NVARCHAR (50)  NULL,
    [Reputation]               FLOAT (53)     NOT NULL,
    [Banned]                   BIT            NOT NULL,
    [VisitCount]               INT            NOT NULL,
    [AvatarPath30]             NVARCHAR (150) NULL,
    [ReputationHonest]         FLOAT (53)     NOT NULL,
    [ReputationConnection]     FLOAT (53)     NULL,
    [ReputationResponsibility] FLOAT (53)     NOT NULL,
    [CountPlus]                INT            NOT NULL,
    [CountMinus]               INT            NOT NULL,
    [Subscription]             BIT            NOT NULL,
    [Signature]                NVARCHAR (500) NULL,
    [MoneyGold]                FLOAT (53)     NOT NULL,
    [MoneyWood]                FLOAT (53)     NOT NULL,
    [MoneyCrystal]             FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID] ASC)
);

