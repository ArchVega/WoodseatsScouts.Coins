const baseUri = 'http://localhost:7167';

const Uris = {
    appState: `${baseUri}/AppState`,

    pointValueFromCode: function (coinQrCode, memberQrCode) {
        return `${baseUri}/home/GetPointValueFromCode?code=${coinQrCode}&memberCode=${memberQrCode}`
    },

    addPointsToMember: `${baseUri}/home/AddPointsToMember`,

    leaderboard: `${baseUri}/home/Report`,

    member: function(memberQrCode) {
        return `${baseUri}/home/GetMemberInfoFromCode?code=${memberQrCode}`
    },

    members: `${baseUri}/home/GetMembersWithPoints`
}

export default Uris