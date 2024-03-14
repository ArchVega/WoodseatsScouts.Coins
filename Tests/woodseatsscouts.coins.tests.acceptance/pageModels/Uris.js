const baseUri = "http://localhost:7167"

const Uris = {
    sutMembers: `${baseUri}/Sut/GetMembers`,
    coinsGet: `${baseUri}/Sut/Coins`,
    sutSetMemberPropertyHasImageToTrue: `${baseUri}/Sut/SetAllMemberHasImagePropertyToTrue`,
    adminCreateMember: `${baseUri}/Admin/CreateMember`,
    adminCreateTroop: `${baseUri}/Admin/CreateTroop`,
    sutScavengerHuntDeadline: (daysToAdd) => `${baseUri}/Sut/SetReportDeadline/${daysToAdd}`
}

export default Uris