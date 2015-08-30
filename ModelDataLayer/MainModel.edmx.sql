
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/30/2015 00:02:29
-- Generated from EDMX file: E:\Projects\ModelBinding\ModelDataLayer\MainModel.edmx
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
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'mmProgramIndicators'
CREATE TABLE [dbo].[mmProgramIndicators] (
    [Id] uniqueidentifier  NOT NULL,
    [ProgramId] uniqueidentifier  NOT NULL,
    [IndicatorId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Indicators'
CREATE TABLE [dbo].[Indicators] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Programs_Governmentprogram'
CREATE TABLE [dbo].[Programs_Governmentprogram] (
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Programs_Subprogram'
CREATE TABLE [dbo].[Programs_Subprogram] (
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

-- Creating primary key on [Id] in table 'mmProgramIndicators'
ALTER TABLE [dbo].[mmProgramIndicators]
ADD CONSTRAINT [PK_mmProgramIndicators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Indicators'
ALTER TABLE [dbo].[Indicators]
ADD CONSTRAINT [PK_Indicators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Programs_Governmentprogram'
ALTER TABLE [dbo].[Programs_Governmentprogram]
ADD CONSTRAINT [PK_Programs_Governmentprogram]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Programs_Subprogram'
ALTER TABLE [dbo].[Programs_Subprogram]
ADD CONSTRAINT [PK_Programs_Subprogram]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ProgramId] in table 'mmProgramIndicators'
ALTER TABLE [dbo].[mmProgramIndicators]
ADD CONSTRAINT [FK_mmProgramIndicatorProgram]
    FOREIGN KEY ([ProgramId])
    REFERENCES [dbo].[Programs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_mmProgramIndicatorProgram'
CREATE INDEX [IX_FK_mmProgramIndicatorProgram]
ON [dbo].[mmProgramIndicators]
    ([ProgramId]);
GO

-- Creating foreign key on [IndicatorId] in table 'mmProgramIndicators'
ALTER TABLE [dbo].[mmProgramIndicators]
ADD CONSTRAINT [FK_IndicatormmProgramIndicator]
    FOREIGN KEY ([IndicatorId])
    REFERENCES [dbo].[Indicators]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IndicatormmProgramIndicator'
CREATE INDEX [IX_FK_IndicatormmProgramIndicator]
ON [dbo].[mmProgramIndicators]
    ([IndicatorId]);
GO

-- Creating foreign key on [Id] in table 'Programs_Governmentprogram'
ALTER TABLE [dbo].[Programs_Governmentprogram]
ADD CONSTRAINT [FK_Governmentprogram_inherits_Program]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Programs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Programs_Subprogram'
ALTER TABLE [dbo].[Programs_Subprogram]
ADD CONSTRAINT [FK_Subprogram_inherits_Program]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Programs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------