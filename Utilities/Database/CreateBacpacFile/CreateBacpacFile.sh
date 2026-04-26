sqlpackage \
    /Action:Export \
    /SourceServerName:"localhost,1433" \
    /SourceDatabaseName:"WoodseatsScouts.Coins.Development" \
    /SourceUser:"sa" \
    /SourcePassword:"Pa55w0rd123" \
    /TargetFile:"/home/developer/dev/bkps/WoodseatsScouts.Coins.Development-alpha.bacpac" \
    /SourceEncryptConnection:False \
    /SourceTrustServerCertificate:True
