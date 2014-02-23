
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/23/2014 14:02:39
-- Generated from EDMX file: C:\Users\Sasha\documents\visual studio 2013\Projects\Rose.VExtension\Rose.VExtension.Server\Models\DbInteraction\Plugins.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-Rose.VExtension.Server-20140131033405];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PluginResourceToken]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ResourceTokens] DROP CONSTRAINT [FK_PluginResourceToken];
GO
IF OBJECT_ID(N'[dbo].[FK_PluginStorageItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StorageItems] DROP CONSTRAINT [FK_PluginStorageItem];
GO
IF OBJECT_ID(N'[dbo].[FK_PluginPluginFileSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PluginFileSystems] DROP CONSTRAINT [FK_PluginPluginFileSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_PluginPluginPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PluginPackages] DROP CONSTRAINT [FK_PluginPluginPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_PluginTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransactionSet] DROP CONSTRAINT [FK_PluginTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_LocalPluginFileSystem_inherits_PluginFileSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PluginFileSystems_LocalPluginFileSystem] DROP CONSTRAINT [FK_LocalPluginFileSystem_inherits_PluginFileSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_LocalStoragePluginPackage_inherits_PluginPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PluginPackages_LocalStoragePluginPackage] DROP CONSTRAINT [FK_LocalStoragePluginPackage_inherits_PluginPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_StreamPackage_inherits_PluginPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PluginPackages_StreamPackage] DROP CONSTRAINT [FK_StreamPackage_inherits_PluginPackage];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Plugins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Plugins];
GO
IF OBJECT_ID(N'[dbo].[ResourceTokens]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResourceTokens];
GO
IF OBJECT_ID(N'[dbo].[StorageItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StorageItems];
GO
IF OBJECT_ID(N'[dbo].[PluginAssociationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PluginAssociationSet];
GO
IF OBJECT_ID(N'[dbo].[PluginFileSystems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PluginFileSystems];
GO
IF OBJECT_ID(N'[dbo].[PluginPackages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PluginPackages];
GO
IF OBJECT_ID(N'[dbo].[TransactionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionSet];
GO
IF OBJECT_ID(N'[dbo].[PluginFileSystems_LocalPluginFileSystem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PluginFileSystems_LocalPluginFileSystem];
GO
IF OBJECT_ID(N'[dbo].[PluginPackages_LocalStoragePluginPackage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PluginPackages_LocalStoragePluginPackage];
GO
IF OBJECT_ID(N'[dbo].[PluginPackages_StreamPackage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PluginPackages_StreamPackage];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Plugins'
CREATE TABLE [dbo].[Plugins] (
    [Id] nvarchar(16)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Location] int  NOT NULL,
    [Version] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ResourceTokens'
CREATE TABLE [dbo].[ResourceTokens] (
    [Id] nvarchar(16)  NOT NULL,
    [PluginId] nvarchar(16)  NOT NULL,
    [ResourceName] nvarchar(max)  NOT NULL,
    [Lifetime] datetime  NOT NULL
);
GO

-- Creating table 'StorageItems'
CREATE TABLE [dbo].[StorageItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [PluginId] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'PluginAssociationSet'
CREATE TABLE [dbo].[PluginAssociationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Seed] nvarchar(max)  NOT NULL,
    [PluginId] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'PluginFileSystems'
CREATE TABLE [dbo].[PluginFileSystems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PluginId] nvarchar(16)  NOT NULL,
    [PluginPluginFileSystem_PluginFileSystem_Id] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'PluginPackages'
CREATE TABLE [dbo].[PluginPackages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PluginId] nvarchar(16)  NOT NULL,
    [PluginPluginPackage_PluginPackage_Id] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'TransactionSet'
CREATE TABLE [dbo].[TransactionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PluginId] nvarchar(16)  NOT NULL
);
GO

-- Creating table 'PluginFileSystems_LocalPluginFileSystem'
CREATE TABLE [dbo].[PluginFileSystems_LocalPluginFileSystem] (
    [RootFolder] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PluginPackages_LocalStoragePluginPackage'
CREATE TABLE [dbo].[PluginPackages_LocalStoragePluginPackage] (
    [RootPath] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PluginPackages_StreamPackage'
CREATE TABLE [dbo].[PluginPackages_StreamPackage] (
    [StreamFileUri] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Plugins'
ALTER TABLE [dbo].[Plugins]
ADD CONSTRAINT [PK_Plugins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ResourceTokens'
ALTER TABLE [dbo].[ResourceTokens]
ADD CONSTRAINT [PK_ResourceTokens]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StorageItems'
ALTER TABLE [dbo].[StorageItems]
ADD CONSTRAINT [PK_StorageItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PluginAssociationSet'
ALTER TABLE [dbo].[PluginAssociationSet]
ADD CONSTRAINT [PK_PluginAssociationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PluginFileSystems'
ALTER TABLE [dbo].[PluginFileSystems]
ADD CONSTRAINT [PK_PluginFileSystems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PluginPackages'
ALTER TABLE [dbo].[PluginPackages]
ADD CONSTRAINT [PK_PluginPackages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransactionSet'
ALTER TABLE [dbo].[TransactionSet]
ADD CONSTRAINT [PK_TransactionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PluginFileSystems_LocalPluginFileSystem'
ALTER TABLE [dbo].[PluginFileSystems_LocalPluginFileSystem]
ADD CONSTRAINT [PK_PluginFileSystems_LocalPluginFileSystem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PluginPackages_LocalStoragePluginPackage'
ALTER TABLE [dbo].[PluginPackages_LocalStoragePluginPackage]
ADD CONSTRAINT [PK_PluginPackages_LocalStoragePluginPackage]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PluginPackages_StreamPackage'
ALTER TABLE [dbo].[PluginPackages_StreamPackage]
ADD CONSTRAINT [PK_PluginPackages_StreamPackage]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PluginId] in table 'ResourceTokens'
ALTER TABLE [dbo].[ResourceTokens]
ADD CONSTRAINT [FK_PluginResourceToken]
    FOREIGN KEY ([PluginId])
    REFERENCES [dbo].[Plugins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PluginResourceToken'
CREATE INDEX [IX_FK_PluginResourceToken]
ON [dbo].[ResourceTokens]
    ([PluginId]);
GO

-- Creating foreign key on [PluginId] in table 'StorageItems'
ALTER TABLE [dbo].[StorageItems]
ADD CONSTRAINT [FK_PluginStorageItem]
    FOREIGN KEY ([PluginId])
    REFERENCES [dbo].[Plugins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PluginStorageItem'
CREATE INDEX [IX_FK_PluginStorageItem]
ON [dbo].[StorageItems]
    ([PluginId]);
GO

-- Creating foreign key on [PluginPluginFileSystem_PluginFileSystem_Id] in table 'PluginFileSystems'
ALTER TABLE [dbo].[PluginFileSystems]
ADD CONSTRAINT [FK_PluginPluginFileSystem]
    FOREIGN KEY ([PluginPluginFileSystem_PluginFileSystem_Id])
    REFERENCES [dbo].[Plugins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PluginPluginFileSystem'
CREATE INDEX [IX_FK_PluginPluginFileSystem]
ON [dbo].[PluginFileSystems]
    ([PluginPluginFileSystem_PluginFileSystem_Id]);
GO

-- Creating foreign key on [PluginPluginPackage_PluginPackage_Id] in table 'PluginPackages'
ALTER TABLE [dbo].[PluginPackages]
ADD CONSTRAINT [FK_PluginPluginPackage]
    FOREIGN KEY ([PluginPluginPackage_PluginPackage_Id])
    REFERENCES [dbo].[Plugins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PluginPluginPackage'
CREATE INDEX [IX_FK_PluginPluginPackage]
ON [dbo].[PluginPackages]
    ([PluginPluginPackage_PluginPackage_Id]);
GO

-- Creating foreign key on [PluginId] in table 'TransactionSet'
ALTER TABLE [dbo].[TransactionSet]
ADD CONSTRAINT [FK_PluginTransaction]
    FOREIGN KEY ([PluginId])
    REFERENCES [dbo].[Plugins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PluginTransaction'
CREATE INDEX [IX_FK_PluginTransaction]
ON [dbo].[TransactionSet]
    ([PluginId]);
GO

-- Creating foreign key on [Id] in table 'PluginFileSystems_LocalPluginFileSystem'
ALTER TABLE [dbo].[PluginFileSystems_LocalPluginFileSystem]
ADD CONSTRAINT [FK_LocalPluginFileSystem_inherits_PluginFileSystem]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PluginFileSystems]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PluginPackages_LocalStoragePluginPackage'
ALTER TABLE [dbo].[PluginPackages_LocalStoragePluginPackage]
ADD CONSTRAINT [FK_LocalStoragePluginPackage_inherits_PluginPackage]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PluginPackages]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PluginPackages_StreamPackage'
ALTER TABLE [dbo].[PluginPackages_StreamPackage]
ADD CONSTRAINT [FK_StreamPackage_inherits_PluginPackage]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PluginPackages]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------