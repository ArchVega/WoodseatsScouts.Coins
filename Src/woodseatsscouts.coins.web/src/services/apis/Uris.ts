import {logApi} from "../../components/logging/Logger.ts";
import getAppSettings from "../../AppSettings.ts";

const baseUri = getAppSettings().VITE_WEB_API_URI + "/api"

const Uris = {
  testDataCoins: `${baseUri}/system/tests/coins`,

  application: () => {
    const resourcePath = `${baseUri}/application`

    return {
      appVersion: () => logApi(`${resourcePath}/app-version`),
      mode: () => logApi(`${resourcePath}/mode`)
    }
  },

  activities: () => {
    const resourcePath = `${baseUri}/application`

    return {
      bases: () => logApi(`${baseUri}/activities/bases`),
    }
  },

  coins: () => {
    const resourcePath = `${baseUri}/coins`

    return {
      resourcePath: resourcePath,
      scans: (coinCode: string, memberCode: string) => logApi(`${resourcePath}/${coinCode}/scans/${memberCode}`),
    }
  },

  scouts: () => {
    const scoutsResourcePath = `${baseUri}/scouts`

    return {
      members: () => {
        const membersResourcePath = `${scoutsResourcePath}/members`
        return {
          memberByCode: function (memberCode: string) {
            return logApi(`${membersResourcePath}/${memberCode}`)
          },
          membersWithPointSummary: function () {
            return logApi(`${membersResourcePath}?view=PointsSummary`);
          },
          fetchMemberComplete: function (memberCode: string) {
            return logApi(`${membersResourcePath}/${memberCode}?view=3`);
          },
          addPointsToMember: function (memberId: number) {
            return `${membersResourcePath}/${memberId}/Coins`
          },
          updateMemberPhoto: function (memberId: number) {
            return `${membersResourcePath}/${memberId}/Photo`
          },
          memberPhoto: function (photoImagePath: string) {
            return `${baseUri}/${photoImagePath}`
          },
          memberName: function (id) {
            return `${baseUri}/Members/${id}/Name?`
          }
        }
      },
      groups: () => {
        const groupsResourcePath = `${scoutsResourcePath}/groups`

        return {
          resourcePath: groupsResourcePath,
        }
      },

      sections: () => {
        const sectionsResourcePath = `${scoutsResourcePath}/sections`

        return {
          resourcePath: sectionsResourcePath,
        }
      }
    }
  },

  scans: () => {
    const resourcePath = `${baseUri}/scans`
    return {
      sessionsLatest: function () {
        return logApi(`${resourcePath}/sessions/latest`)
      },
    }
  },
}

export default Uris