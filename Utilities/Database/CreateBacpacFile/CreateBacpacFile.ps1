sqlpackage \
    /Action:Export \
    /SourceServerName:"localhost,1433" \
    /SourceDatabaseName:"WoodseatsScouts.Coins.Development" \
    /SourceUser:"sa" \
    /SourcePassword:"Pa55w0rd123" \
    /TargetFile:"WoodseatsScouts.Coins.Development.bacpac" \
    /SourceEncryptConnection:False \
    /SourceTrustServerCertificate:True
