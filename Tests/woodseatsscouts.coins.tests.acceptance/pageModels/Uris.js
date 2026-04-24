const baseUri = "http://localhost:7167"

const Uris = {
    adminCreateMember: `${baseUri}/Admin/Member`,
    adminCreateScoutGroup: `${baseUri}/Admin/ScoutGroup`,
    sutMembers: `${baseUri}/Sut/Members`,
    coinsGet: `${baseUri}/Sut/Coins`,
    sutSetMemberPropertyHasImageToTrue: `${baseUri}/Sut/Members/HasImage/True`,
    memberPhoto: function(member) {
        return `${baseUri}/Members/${member.id}/Photo`
    },
    sutScavengerHuntDeadline: (minutesToAdd) => `${baseUri}/Sut/Leaderboard/Deadline/${minutesToAdd}`,
    sutSystemDateTime: (minutesToAdd) => {
        if (minutesToAdd === undefined || minutesToAdd === null) {
            return `${baseUri}/Sut/SystemDateTime`
        } else {
            return `${baseUri}/Sut/SystemDateTime/${minutesToAdd}`
        }
    }
}

export default Uris