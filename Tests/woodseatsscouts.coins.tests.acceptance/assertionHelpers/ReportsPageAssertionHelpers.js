import {expect} from "@playwright/test";
import ReportPage from "../pageModels/ReportPage";

const assertReportsPageIs = async (page, last3ScannedMembers, top3Groups, leaderboard) => {
    const reportPage = ReportPage(page);
    await reportPage.goTo()

    await assertGroupLeaderboardGroupsAre(leaderboard, reportPage)
    await assertGetLast3ScannedMembers(last3ScannedMembers, reportPage)
    await assertTop3GroupsAre(top3Groups, reportPage)
}

const assertGetLast3ScannedMembers = async (expectedMembers, reportPage) => {
    const actualMembers = await reportPage.getLast3ScannedMembers()
    expect(actualMembers.length).toBe(expectedMembers.length)
    for (let i = 0; i < expectedMembers.length; i++) {
        const expectedMember = expectedMembers[i];
        const actualMember = actualMembers[i];
        expect(actualMember).toEqual(expect.objectContaining(expectedMember));
    }
}

const assertTop3GroupsAre = async (expectedGroups, reportPage) => {
    const actualGroups = await reportPage.getTopThreeGroups()
    expect(actualGroups.length).toBe(expectedGroups.length)
    for (let i = 0; i < expectedGroups.length; i++) {
        const expectedGroup = expectedGroups[i];
        const actualGroup = actualGroups[i];
        expect(actualGroup).toEqual(expect.objectContaining(expectedGroup));
    }
}

const assertGroupLeaderboardGroupsAre = async (expectedLeaderboardGroups, reportPage) =>
{
    const actualLeaderboardGroups = await reportPage.getGroupLeaderboard()
    expect(actualLeaderboardGroups.length).toBe(expectedLeaderboardGroups.length)
    for (let i = 0; i < expectedLeaderboardGroups.length; i++) {
        const expectedLeaderboardGroup = expectedLeaderboardGroups[i];
        const actualLeaderboardGroup = actualLeaderboardGroups[i];
        expect(actualLeaderboardGroup).toEqual(expect.objectContaining(expectedLeaderboardGroup));
    }
}

export {assertReportsPageIs}