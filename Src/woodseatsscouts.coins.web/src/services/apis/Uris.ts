import {logApi} from "../../components/logging/Logger.ts";

const baseUri = import.meta.env.VITE_WEB_API_URI + "/api"

const Uris = {
  member: `${baseUri}/members`,

  appState: `${baseUri}/AppState`,
  appVersion: `${baseUri}/AppState/AppVersion`,
  leaderboard: `${baseUri}/Leaderboard/Report`,
  refreshSecondsForLatestScans: `${baseUri}/Members/RefreshSecondsForLatestScans`,
  testDataCoins: `${baseUri}/Sut/Coins`,
  latest6Scavengers: `${baseUri}/Members/LatestScans`,

  memberByCode: function (memberCode: string) {
    return logApi(`${this.member}/${memberCode}`)
  },
  memberPhoto: function (photoImagePath: string) {
    return `${baseUri}/${photoImagePath}`
  },
  pointValueFromCode: function (coinCode: string, memberCode: string) {
    return `${baseUri}/Coins/${coinCode}/Scan/${memberCode}`
  },



  addPointsToMember: function (id) {
    return `${baseUri}/Members/${id}/Coins`
  },
  memberWithPoints: function (memberQrCode) {
    return `${baseUri}/Members/${memberQrCode}/WithPoints`
  },
  memberName: function (id) {
    return `${baseUri}/Members/${id}/Name?`
  }
}

export default Uris