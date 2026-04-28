alter table dbo.ActivityBases
    alter column Name nvarchar(max) not null
go

-- don't know how to alter type of dbo.Coins.Code

-- don't know how to alter default on dbo.Coins.Code

drop index IX_Coins_Code on dbo.Coins
go

alter table dbo.ErrorLogs
    drop column Path
go

alter table dbo.ErrorLogs
    drop column Method
go

create table dbo.ScanCoins
(
    Id            int identity
        constraint PK_ScanCoins
            primary key,
    ScanSessionId int not null
        constraint FK_ScanCoins_ScanSessions_ScanSessionId
            references dbo.ScanSessions
            on delete cascade,
    CoinId        int not null
        constraint FK_ScanCoins_Coins_CoinId
            references dbo.Coins
            on delete cascade
)
go

create index IX_ScanCoins_CoinId
    on dbo.ScanCoins (CoinId)
go

create index IX_ScanCoins_ScanSessionId
    on dbo.ScanCoins (ScanSessionId)
go

drop table dbo.ScannedCoins
go

alter table dbo.ScoutGroups
    alter column Name nvarchar(max) not null
go

-- don't know how to alter type of dbo.ScoutMembers.Code

-- don't know how to alter default on dbo.ScoutMembers.Code

alter table dbo.ScoutMembers
    alter column FirstName nvarchar(max) not null
go

alter table dbo.ScoutMembers
    alter column LastName nvarchar(max) null
go

alter table dbo.ScoutMembers
    add ScoutSectionId char not null
        constraint FK_ScoutMembers_ScoutSections_ScoutSectionId
            references dbo.ScoutSections
            on delete cascade
go

-- column reordering is not supported dbo.ScoutMembers.ScoutSectionId

alter table dbo.ScoutMembers
    alter column Clue1State nvarchar(max) null
go

alter table dbo.ScoutMembers
    alter column Clue2State nvarchar(max) null
go

alter table dbo.ScoutMembers
    alter column Clue3State nvarchar(max) null
go

drop index IX_ScoutMembers_Code on dbo.ScoutMembers
go

create index IX_ScoutMembers_ScoutSectionId
    on dbo.ScoutMembers (ScoutSectionId)
go

drop index IX_ScoutMembers_ScoutSectionCode on dbo.ScoutMembers
go

alter table dbo.ScoutMembers
    drop constraint FK_ScoutMembers_ScoutSections_ScoutSectionCode
go

alter table dbo.ScoutMembers
    drop column ScoutSectionCode
go


        alter VIEW [ScoutMembersSessionCoins] AS
        SELECT
            ScoutMembers.FirstName as 'Scout Member First Name',
            ScoutMembers.LastName as 'Scout Member Last Name',
            ScoutMembers.Code as 'Scout Member Code',
            ScoutMembers.Number as 'Scout Member Number',
            ScoutMembers.HasImage as 'Scout Member Has Image',
            ScoutGroups.Name as 'Scout Group',
            ScoutSections.Name as 'Scout Section',
            Coins.Code as 'Coin Code',
            Coins.Value as 'Coin Value',
            ActivityBases.Name as 'Activity Base'
        FROM ScoutMembers
        join ScanSessions
        on ScanSessions.ScoutMemberId = ScoutMembers.Id
        join ScanCoins
        on ScanCoins.ScanSessionId = ScanSessions.Id
        join ScoutGroups
        on ScoutGroups.Id = ScoutMembers.ScoutGroupId
        join ScoutSections
        on ScoutSections.Code = ScoutMembers.ScoutSectionId
        join Coins
        on Coins.Id = ScanCoins.CoinId
        join ActivityBases
        on ActivityBases.Id = Coins.ActivityBaseId
go

