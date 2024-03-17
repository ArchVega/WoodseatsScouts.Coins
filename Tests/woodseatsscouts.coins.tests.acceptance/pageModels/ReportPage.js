import {expect} from "@playwright/test";

const ReportPage = (page) => {
    let top3GroupsTable = null;
    let groupLeaderboardTable = null;
    let countdownClockDiv = null;
    let last3ScannedMembersDiv = null;

    return {
        goTo: async (code) => {
            await page.goto('/leaderboard');

            top3GroupsTable = page.getByTestId("table-top-3-groups")
            groupLeaderboardTable = page.getByTestId("table-group-leaderboard")
            countdownClockDiv = page.getByTestId("div-countdown-clock")
            last3ScannedMembersDiv = page.getByTestId("div-last-3-scanned-members")

            await expect(top3GroupsTable).toBeVisible()
            await expect(groupLeaderboardTable).toBeVisible()
            await expect(countdownClockDiv).toBeVisible()
            await expect(last3ScannedMembersDiv).toBeVisible()
        },

        getLast3ScannedMembers: async () => {
            const latest3ScannedUsers = await last3ScannedMembersDiv.getByTestId("latest-3-scanned-user").all()

            return await Promise.all(latest3ScannedUsers.map(async member => {
                return {
                    // userId: Number(await member.getAttribute("data-userid")),
                    userName: await member.getByTestId("latest-3-scanned-user-name").textContent(),
                    troopName: await member.getByTestId("latest-3-scanned-troop-name").textContent(),
                    sectionName: await member.getByTestId("latest-3-scanned-section-name").textContent(),
                    userPoints: Number(await member.getByTestId("latest-3-scanned-user-points").textContent())
                }
            }))
        },

        getTopThreeGroups: async () => {
            const top3Groups = await top3GroupsTable.getByTestId("last-hour-top-group").all()

            return await Promise.all(top3Groups.map(async member => {
                return {
                    groupName: await member.getByTestId("last-hour-top-group-name").textContent(),
                    averagePoints: Number(await member.getByTestId("last-hour-top-group-average-points").textContent())
                }
            }))
        },
        getGroupLeaderboard: async () => {
            const top3Groups = await groupLeaderboardTable.getByTestId("group-leaderboard-group").all()

            return await Promise.all(top3Groups.map(async member => {
                return {
                    groupName: await member.getByTestId("group-leaderboard-group-name").textContent(),
                    averagePoints: Number(await member.getByTestId("group-leaderboard-group-average-points").textContent())
                }
            }))
        },

        getHoursLeft: async () => {
            const hoursLeftLocator = page.getByTestId("div-countdown-clock").locator(".hours")
            return Number(await hoursLeftLocator.textContent())
        }
    }
}

export default ReportPage