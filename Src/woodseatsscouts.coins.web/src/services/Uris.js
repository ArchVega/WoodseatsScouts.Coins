const baseUri = process.env.REACT_APP_WEB_API_URI

const Uris = {
  appState: `${baseUri}/AppState`,
  appVersion: `${baseUri}/AppState/AppVersion`,
  members: `${baseUri}/Members`,
  leaderboard: `${baseUri}/Leaderboard/Report`,
  testDataCoins: `${baseUri}/Sut/Coins`,
  countries: `${baseUri}/Countries`,
  voteResults: `${baseUri}/Vote/Results`,


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
    return `${baseUri}/Members/${id}/Photo`
  },
  registerVoteForMember(memberId, countryId) {
    return `${baseUri}/Vote/${memberId}/RegisterVote?countryId=${countryId}`
  }
}

export default Uris