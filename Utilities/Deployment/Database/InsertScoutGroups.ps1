function GetScoutGroups() {
    $data = "
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (46, N'St Pauls');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (69, N'Mosborough');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (74, N'Oak Street');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (99, N'Woodseats Explorers');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (146, N'Old Norton');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (181, N'St Chads');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (229, N'Greenhill Methodist');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (246, N'Beauchief');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (265, N'Greenhill');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (280, N'Norton');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (297, N'Bradway');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (500, N'229th Greenhill');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (501, N'Woodseats Explorers');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (502, N'270th Intake');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (503, N'280th Norton');
INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name) VALUES (504, N'92nd Woodhouse');
"

    return "SET IDENTITY_INSERT dbo.ScoutGroups ON;
    $data
    SET IDENTITY_INSERT dbo.ScoutGroups OFF;"    
}
