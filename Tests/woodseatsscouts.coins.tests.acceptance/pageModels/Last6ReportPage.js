import {expect} from "@playwright/test";

const Last6ReportPage = (page) => {
    let last6ScannedMembersDiv = null;

    return {
        goTo: async () => {
            await page.goto('/leaderboard/members');
            last6ScannedMembersDiv = page.locator("#leaderboards-members")
            await expect(last6ScannedMembersDiv).toBeVisible()
        },

        getLast6ScannedMembers: async () => {
            const latest6ScannedUsers = await last6ScannedMembersDiv.locator(".member-info").all()

            return await Promise.all(latest6ScannedUsers.map(async member => {
                return {
                    userName: await member.getByTestId("latest-6-scanned-user-name").textContent(),
                    sectionName: await member.getByTestId("latest-6-scanned-section-name").textContent(),
                    userPoints: Number(await member.getByTestId("latest-6-scanned-user-points").textContent())
                }
            }))
        }
    }
}

export default Last6ReportPage