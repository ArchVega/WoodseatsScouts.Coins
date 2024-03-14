const baseUri = "http://localhost:7167"

const Uris = {
    sutMembers: `${baseUri}/Sut/GetMembers`,
    coinsGet: `${baseUri}/Sut/Coins`,
    sutSetMemberPropertyHasImageToTrue: `${baseUri}/Sut/SetAllMemberHasImagePropertyToTrue`,
    adminCreateMember: `${baseUri}/Admin/Member`,
    adminCreateTroop: `${baseUri}/Admin/Troop`,
    sutScavengerHuntDeadline: (daysToAdd) => `${baseUri}/Sut/SetReportDeadline/${daysToAdd}`
}

export default Uris