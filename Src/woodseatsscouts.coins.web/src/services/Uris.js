const baseUri = process.env.REACT_APP_WEB_API_URI

const Uris = {
    appState: `${baseUri}/AppState`,
    members: `${baseUri}/Member/GetMembersWithPoints`,
    addPointsToMember: `${baseUri}/Member/AddPointsToMember`,
    leaderboard: `${baseUri}/Leaderboard/Report`,

    pointValueFromCode: function (coinQrCode, memberQrCode) {
        return `${baseUri}/Coin/GetPointValueFromCode?code=${coinQrCode}&memberCode=${memberQrCode}`
    },
    member: function (memberQrCode) {
        return `${baseUri}/Member/GetMemberInfoFromCode?code=${memberQrCode}`
    },
}

export default Uris