const baseUri = "http://localhost:7167"

const Uris = {
    adminCreateMember: `${baseUri}/api/scouts/members`,
    adminCreateScoutGroup: `${baseUri}/api/scouts/groups`,
    sutMembers: `${baseUri}/api/system/tests/members`,
    coinsGet: `${baseUri}/api/system/tests/coins`,
    sutSetMemberPropertyHasImageToTrue: `${baseUri}/api/system/tests/members/has-image/true`,
    memberPhoto: function(member) {
        return `${baseUri}/api/scouts/members/${member.id}/photo`
    },
    // sutSystemDateTime: (minutesToAdd) => {
    //     if (minutesToAdd === undefined || minutesToAdd === null) {
    //         return `${baseUri}/Sut/SystemDateTime`
    //     } else {
    //         return `${baseUri}/Sut/SystemDateTime/${minutesToAdd}`
    //     }
    // }
}

export default Uris