const baseUri = import.meta.env.VITE_WEB_API_URI

const Uris = {
  appState: `${baseUri}/AppState`,
  appVersion: `${baseUri}/AppState/AppVersion`,
  members: `${baseUri}/Members`,
  leaderboard: `${baseUri}/Leaderboard/Report`,
  refreshSecondsForLatestScans: `${baseUri}/Members/RefreshSecondsForLatestScans`,
  testDataCoins: `${baseUri}/Sut/Coins`,
  latest6Scavengers: `${baseUri}/Members/LatestScans`,

  addPointsToMember: function (id) {
    return `${baseUri}/Members/${id}/Coins`
  },
  pointValueFromCode: function (coinQrCode, memberQrCode) {
    return `${baseUri}/Coins/${coinQrCode}/Scan/${memberQrCode}`
  },
  member: function (memberQrCode) {
    return `${baseUri}/Members/${memberQrCode}`
  },
  memberWithPoints: function (memberQrCode) {
    return `${baseUri}/Members/${memberQrCode}/WithPoints`
  },
  memberPhoto: function (id) {
    return `${baseUri}/Members/${id}/Photo?${new Date()}`
  },
  memberName: function (id) {
    return `${baseUri}/Members/${id}/Name?`
  }
}

export default Uris