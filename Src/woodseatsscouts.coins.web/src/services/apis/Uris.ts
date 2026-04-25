import {logApi} from "../../components/logging/Logger.ts";

const Uris = {
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
          memberById: function (id: number) {
            return logApi(`${membersResourcePath}/${id}`)
          },
          membersWithPointSummary: function () {
            return logApi(`${membersResourcePath}`);
          },
          getMemberComplete: function (scoutMemberId: number) {
            return logApi(`${membersResourcePath}/${scoutMemberId}/complete`);
          },
          addPointsToMember: function (memberId: number) {
            return logApi(`${membersResourcePath}/${memberId}/coins`)
          },
          updateMemberPhoto: function (memberId: number) {
            return logApi(`${membersResourcePath}/${memberId}/photo`)
          },
          memberPhoto: function (photoImagePath: string) {
            return logApi(`${scoutsResourcePath}/${photoImagePath}`)
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

  system: () => {
    const resourcePath = `system`
    return {
      testDataCoins: function() {
        return logApi(`${resourcePath}/tests/coins`)
      }
    }
  }
}

export default Uris