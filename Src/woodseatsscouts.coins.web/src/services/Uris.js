const baseUri = process.env.REACT_APP_WEB_API_URI

const Uris = {
  appState: `${baseUri}/AppState`,
  appVersion: `${baseUri}/AppState/AppVersion`,
  members: `${baseUri}/Members`,
  leaderboard: `${baseUri}/Leaderboard/Report`,
  leaderboardLast6ScavengersPageRefreshSeconds: `${baseUri}/Leaderboard/Last6ScavengersPageRefreshSeconds`,
  testDataCoins: `${baseUri}/Sut/Coins`,
  countries: `${baseUri}/Countries`,
  voteResults: `${baseUri}/Vote/Results`,
  latest6Scavengers: `${baseUri}/Leaderboard/Members`,

  addPointsToMember: function (id) {
    return `${baseUri}/Members/${id}/Coins`
  },
  pointValueFromCode: function (coinQrCode, memberQrCode) {
    return `${baseUri}/Coins/${coinQrCode}/Scan/${memberQrCode}`
  },
  member: function (memberQrCode) {
    return `${baseUri}/Members/${memberQrCode}`
  },
  memberForVoting: function (memberQrCode) {
    return `${baseUri}/Members/${memberQrCode}/Vote`
  },
  memberPhoto: function (id) {
    return `${baseUri}/Members/${id}/Photo?${new Date()}`
  },
  registerVoteForMember(memberId, countryId) {
    return `${baseUri}/Vote/${memberId}/RegisterVote?countryId=${countryId}`
  },
}

export default Uris