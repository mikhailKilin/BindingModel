
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/29/2015 21:51:56
-- Generated from EDMX file: E:\Projects\ModelBinding\DataLayer\MainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ModelBinding];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Programs'
CREATE TABLE [dbo].[Programs] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Indicators'
CREATE TABLE [dbo].[Indicators] (
    [Id] uniqueidentifier  NOT NULL,
    [ProgramId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Programs_Subprogram'
CREATE TABLE [dbo].[Programs_Subprogram] (
    [GovernmentProgramId] uniqueidentifier  NOT NULL,
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Programs_GovernmentProgram'
CREATE TABLE [dbo].[Programs_GovernmentProgram] (
    [mmGovernmentProgramIndicatorId] uniqueidentifier  NOT NULL,
    [Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Programs'
ALTER TABLE [dbo].[Programs]
ADD CONSTRAINT [PK_Programs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Indicators'
ALTER TABLE [dbo].[Indicators]
ADD CONSTRAINT [PK_Indicators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Programs_Subprogram'
ALTER TABLE [dbo].[Programs_Subprogram]
ADD CONSTRAINT [PK_Programs_Subprogram]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Programs_GovernmentProgram'
ALTER TABLE [dbo].[Programs_GovernmentProgram]
ADD CONSTRAINT [PK_Programs_GovernmentProgram]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [GovernmentProgramId] in table 'Programs_Subprogram'
ALTER TABLE [dbo].[Programs_Subprogram]
ADD CONSTRAINT [FK_SubprogramGovernmentProgram]
    FOREIGN KEY ([GovernmentProgramId])
    REFERENCES [dbo].[Programs_GovernmentProgram]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubprogramGovernmentProgram'
CREATE INDEX [IX_FK_SubprogramGovernmentProgram]
ON [dbo].[Programs_Subprogram]
    ([GovernmentProgramId]);
GO

-- Creating foreign key on [ProgramId] in table 'Indicators'
ALTER TABLE [dbo].[Indicators]
ADD CONSTRAINT [FK_IndicatorProgram]
    FOREIGN KEY ([ProgramId])
    REFERENCES [dbo].[Programs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IndicatorProgram'
CREATE INDEX [IX_FK_IndicatorProgram]
ON [dbo].[Indicators]
    ([ProgramId]);
GO

-- Creating foreign key on [Id] in table 'Programs_Subprogram'
ALTER TABLE [dbo].[Programs_Subprogram]
ADD CONSTRAINT [FK_Subprogram_inherits_Program]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Programs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Programs_GovernmentProgram'
ALTER TABLE [dbo].[Programs_GovernmentProgram]
ADD CONSTRAINT [FK_GovernmentProgram_inherits_Program]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Programs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------