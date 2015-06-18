
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/16/2015 16:47:36
-- Generated from EDMX file: C:\Users\Владимир\Desktop\Course Project\WinNote\WinNote 16.05.2015 final version\WinNote\WinNote\Model\Entity\DataBase\WinModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WinNote];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[LogedInUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogedInUsers];
GO
IF OBJECT_ID(N'[dbo].[Calendars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Calendars];
GO
IF OBJECT_ID(N'[dbo].[CalendarEventsToLoadOnServers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalendarEventsToLoadOnServers];
GO
IF OBJECT_ID(N'[dbo].[CalendarSharedEventActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalendarSharedEventActions];
GO
IF OBJECT_ID(N'[dbo].[NotepadCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotepadCategories];
GO
IF OBJECT_ID(N'[dbo].[NotepadTextDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotepadTextDocuments];
GO
IF OBJECT_ID(N'[dbo].[NotepadCategoryChangeds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotepadCategoryChangeds];
GO
IF OBJECT_ID(N'[dbo].[NotepadTextDocumentsChangeds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotepadTextDocumentsChangeds];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL
);
GO

-- Creating table 'LogedInUsers'
CREATE TABLE [dbo].[LogedInUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Calendars'
CREATE TABLE [dbo].[Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EventName] nvarchar(max)  NOT NULL,
    [DateFrom] nvarchar(max)  NOT NULL,
    [DateTo] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [ShareKey] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [Created] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'CalendarEventsToLoadOnServers'
CREATE TABLE [dbo].[CalendarEventsToLoadOnServers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EventName] nvarchar(max)  NOT NULL,
    [DateFrom] nvarchar(max)  NOT NULL,
    [DateTo] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [ShareKey] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [Created] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CalendarSharedEventActions'
CREATE TABLE [dbo].[CalendarSharedEventActions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SharedKey] nvarchar(max)  NOT NULL,
    [ServerId] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'NotepadCategories'
CREATE TABLE [dbo].[NotepadCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [CategoryName] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [Created] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'NotepadTextDocuments'
CREATE TABLE [dbo].[NotepadTextDocuments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [CategoryId] int  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [Created] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'NotepadCategoryChangeds'
CREATE TABLE [dbo].[NotepadCategoryChangeds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [CategoryName] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [Created] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'NotepadTextDocumentsChangeds'
CREATE TABLE [dbo].[NotepadTextDocumentsChangeds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [CategoryId] int  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [ServerId] int  NOT NULL,
    [Created] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LogedInUsers'
ALTER TABLE [dbo].[LogedInUsers]
ADD CONSTRAINT [PK_LogedInUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [PK_Calendars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CalendarEventsToLoadOnServers'
ALTER TABLE [dbo].[CalendarEventsToLoadOnServers]
ADD CONSTRAINT [PK_CalendarEventsToLoadOnServers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CalendarSharedEventActions'
ALTER TABLE [dbo].[CalendarSharedEventActions]
ADD CONSTRAINT [PK_CalendarSharedEventActions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotepadCategories'
ALTER TABLE [dbo].[NotepadCategories]
ADD CONSTRAINT [PK_NotepadCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotepadTextDocuments'
ALTER TABLE [dbo].[NotepadTextDocuments]
ADD CONSTRAINT [PK_NotepadTextDocuments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotepadCategoryChangeds'
ALTER TABLE [dbo].[NotepadCategoryChangeds]
ADD CONSTRAINT [PK_NotepadCategoryChangeds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotepadTextDocumentsChangeds'
ALTER TABLE [dbo].[NotepadTextDocumentsChangeds]
ADD CONSTRAINT [PK_NotepadTextDocumentsChangeds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------