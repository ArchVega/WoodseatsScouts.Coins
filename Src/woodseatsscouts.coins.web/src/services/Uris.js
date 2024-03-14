const baseUri = process.env.REACT_APP_WEB_API_URI

const Uris = {
    appState: `${baseUri}/AppState`,

    pointValueFromCode: function (coinQrCode, memberQrCode) {
        return `${baseUri}/Coin/GetPointValueFromCode?code=${coinQrCode}&memberCode=${memberQrCode}`
    },

    addPointsToMember: `${baseUri}/Member/AddPointsToMember`,

    leaderboard: `${baseUri}/Leaderboard/Report`,

    member: function (memberQrCode) {
        return `${baseUri}/Member/GetMemberInfoFromCode?code=${memberQrCode}`
    },

    members: `${baseUri}/Member/GetMembersWithPoints`
}

export default Uris