function InsertTestData {
    param(
        [ValidateSet("Developmnet", "IntegrationTest", "AcceptanceTest")]
        $Configuration
    )


    if ($Configuration -eq "Acceptance") {
        CreateCoinData
    }
}