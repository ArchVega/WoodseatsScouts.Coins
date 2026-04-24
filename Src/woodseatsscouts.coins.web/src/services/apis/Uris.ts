import {logApi} from "../../components/logging/Logger.ts";

// const baseUri = import.meta.env.VITE_WEB_API_URI + "/api"
const baseUri = "https://release-scouts-webapi.azurewebsites.net/api"

const Uris = {
  members: `${baseUri}/members`,
  coins: `${baseUri}/coins`,

  appState: `${baseUri}/AppState`,
  appVersion: `${baseUri}/AppState/AppVersion`,
  leaderboard: `${baseUri}/Leaderboard/Report`,
  testDataCoins: `${baseUri}/Sut/Coins`,

  memberByCode: function (memberCode: string) {
    return logApi(`${this.members}/${memberCode}`)
  },
  membersWithPointSummary: function() {
    return logApi(`${this.members}?view=PointsSummary`);
  },
  fetchMemberComplete: function(memberCode: string) {
    return logApi(`${this.members}/${memberCode}?view=3`);
  },
  memberLatestScans: function() {
   return logApi(`${this.members}/LatestScans`)
  },
  pointValueFromCode: function (coinCode: string, memberCode: string) {
    return `${this.coins}/${coinCode}/Scan/${memberCode}`
  },
  addPointsToMember: function (memberId: number) {
    return `${this.members}/${memberId}/Coins`
  },
  updateMemberPhoto: function(memberId: number) {
    return `${this.members}/${memberId}/Photo`
  },

  scoutGroups: function() {
    return `${this.appState}/ScoutGroups`
  },
  sections: function() {
    return `${this.appState}/Sections`
  },




  memberPhoto: function (photoImagePath: string) {
    return `${baseUri}/${photoImagePath}`
  },
  // memberWithPoints: function (memberQrCode) {
  //   return `${baseUri}/Members/${memberQrCode}/WithPoints`
  // },
  memberName: function (id) {
    return `${baseUri}/Members/${id}/Name?`
  }
}

export default Uris