import {logApi} from "../../components/logging/Logger.ts";

const Uris = {
  testDataCoins: `system/tests/coins`,

  application: () => {
    const resourcePath = `application`

    return {
      appVersion: () => logApi(`${resourcePath}/app-version`),
      mode: () => logApi(`${resourcePath}/mode`)
    }
  },

  activities: () => {
    const resourcePath = `application`

    return {
      bases: () => logApi(`activities/bases`),
    }
  },

  coins: () => {
    const resourcePath = `coins`

    return {
      resourcePath: resourcePath,
      assign: (coinCode: string, memberCode: string) => logApi(`${resourcePath}/${coinCode}/assign/${memberCode}`),
    }
  },

  scouts: () => {
    const scoutsResourcePath = `scouts`

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
            return logApi(`${membersResourcePath}/${memberId}/Coins`)
          },
          updateMemberPhoto: function (memberId: number) {
            return logApi(`${membersResourcePath}/${memberId}/Photo`)
          },
          memberPhoto: function (photoImagePath: string) {
            return logApi(`${scoutsResourcePath}/${photoImagePath}`)
          },
          memberName: function (id: number) {
            return logApi(`${membersResourcePath}/${id}/Name?`)
          },
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
    const resourcePath = `scans`
    return {
      sessionsLatest: function () {
        return logApi(`${resourcePath}/sessions/latest`)
      },
    }
  },
}

export default Uris