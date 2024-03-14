const baseUri = "http://localhost:7167"

const Uris = {
    adminCreateMember: `${baseUri}/Admin/Member`,
    adminCreateTroop: `${baseUri}/Admin/Troop`,
    sutMembers: `${baseUri}/Sut/Members`,
    coinsGet: `${baseUri}/Sut/Coins`,
    sutSetMemberPropertyHasImageToTrue: `${baseUri}/Sut/Members/HasImage/True`,
    sutScavengerHuntDeadline: (daysToAdd) => `${baseUri}/Sut/Leaderboard/Deadline/${daysToAdd}`
}

export default Uris