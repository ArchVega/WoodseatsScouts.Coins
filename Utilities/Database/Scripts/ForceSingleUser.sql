USE [WoodseatsScouts.Coins.Tests.Acceptance];
GO

ALTER DATABASE [WoodseatsScouts.Coins.Tests.Acceptance] SET ONLINE;
GO

ALTER DATABASE [WoodseatsScouts.Coins.Tests.Acceptance] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

alter database [WoodseatsScouts.Coins.Tests.Acceptance] set multi_user;
GO

ALTER DATABASE [WoodseatsScouts.Coins.Tests.Acceptance] SET OFFLINE;
GO

