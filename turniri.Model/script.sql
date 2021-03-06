-- SqlDump Database Dump Wizard Ver 3.0 (libmssql version 2.3)
--
-- Host: JUPITER	  Database: turniri
-- ---------------------------------------------------------
-- Server version	Microsoft SQL Server Version 10.50.255


-- Table structure for table 'Language'
IF EXISTS (SELECT * FROM sysobjects WHERE (name = 'Language')) DROP TABLE Language
GO
CREATE TABLE Language (
ID int IDENTITY NOT NULL,
Code nvarchar(50) NOT NULL,
Name nvarchar(50) NOT NULL)
GO

CREATE UNIQUE CLUSTERED INDEX PK_Language ON Language (ID)
GO


-- Dumping data for table 'Language'
--

-- Disable identity insert
SET IDENTITY_INSERT Language OFF
GO

INSERT INTO Language (ID, Code, Name)
VALUES(1, 'ru', 'Русский')
GO
INSERT INTO Language (ID, Code, Name)
VALUES(2, 'en', 'Английский')
GO

-- Enable identity insert
SET IDENTITY_INSERT Language ON
GO


-- Table structure for table 'Role'
IF EXISTS (SELECT * FROM sysobjects WHERE (name = 'Role')) DROP TABLE Role
GO
CREATE TABLE Role (
ID int IDENTITY NOT NULL,
Code nvarchar(50) NOT NULL,
Name nvarchar(50) NOT NULL)
GO

CREATE UNIQUE CLUSTERED INDEX PK_Role ON Role (ID)
GO


-- Dumping data for table 'Role'
--

-- Disable identity insert
SET IDENTITY_INSERT Role OFF
GO


INSERT INTO Role (ID, Code, Name)
VALUES(1, 'admin', 'Админ')
GO

-- Enable identity insert
SET IDENTITY_INSERT Role ON
GO



-- Table structure for table '[User]'
IF EXISTS (SELECT * FROM sysobjects WHERE (name = 'User')) DROP TABLE [User]
GO
CREATE TABLE [User] (
ID int IDENTITY NOT NULL,
Email nvarchar(150) NOT NULL,
Password nvarchar(50) NOT NULL,
LanguageID int)
GO

CREATE UNIQUE CLUSTERED INDEX PK_User ON [User] (ID)
GO


-- Dumping data for table '[User]'
--


-- Disable identity insert
SET IDENTITY_INSERT [User] OFF
GO

INSERT INTO [User] (ID, Email, Password, LanguageID)
VALUES(1, 'admin', 'admin', NULL)
GO

-- Enable identity insert
SET IDENTITY_INSERT [User] ON
GO



-- Table structure for table 'UserRole'
IF EXISTS (SELECT * FROM sysobjects WHERE (name = 'UserRole')) DROP TABLE UserRole
GO
CREATE TABLE UserRole (
ID int IDENTITY NOT NULL,
RoleID int NOT NULL,
UserID int NOT NULL)
GO

CREATE UNIQUE CLUSTERED INDEX PK_UserRole ON UserRole (ID)
GO


-- Dumping data for table 'UserRole'
--

-- Disable identity insert
SET IDENTITY_INSERT UserRole OFF
GO


INSERT INTO UserRole (ID, RoleID, UserID)
VALUES(1, 1, 1)
GO

-- Enable identity insert
SET IDENTITY_INSERT UserRole ON
GO


