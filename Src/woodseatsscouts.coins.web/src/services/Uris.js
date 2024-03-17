const baseUri = process.env.REACT_APP_WEB_API_URI

const Uris = {
    appState: `${baseUri}/AppState`,
    members: `${baseUri}/Members`,
    leaderboard: `${baseUri}/Leaderboard/Report`,
    testDataCoins: `${baseUri}/Sut/Coins`,
    memberPhoto: function(id) {
        return `${baseUri}/Members/${id}/Photo'`
    },
    addPointsToMember: function(id) {
        return `${baseUri}/Members/${id}/Coins`
    },
    pointValueFromCode: function (coinQrCode, memberQrCode) {
        return `${baseUri}/Coins/${coinQrCode}/Scan/${memberQrCode}`
    },
    member: function (memberQrCode) {
        return `${baseUri}/Members/${memberQrCode}`
    },
}

export default Uris