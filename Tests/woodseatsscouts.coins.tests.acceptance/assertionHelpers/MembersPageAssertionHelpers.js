import MembersPage from "../pageModels/MembersPage";
import {expect} from "@playwright/test";

const assertMembersPageContains = async (page, expectedMembers) => {
    const membersPage = MembersPage(page);
    await membersPage.goTo()

    await membersPage.showTotalPoints(true);

    const actualMembers = await membersPage.getAllMembers()

    for (let i = 0; i < expectedMembers.length; i++) {
        const expectedMember = expectedMembers[i];
        const actualMember = actualMembers.filter(actualMember => actualMember.name === expectedMember.name)[0]

        expect(actualMember).toEqual(expect.objectContaining(expectedMember));
    }
}

export {assertMembersPageContains}