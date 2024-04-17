import {expect} from "@playwright/test";
import ReportPage from "../pageModels/ReportPage";
import Last6ReportPage from "../pageModels/Last6ReportPage";

const last6ScannersAssertionHelpers = async (page, last6ScannedMembers) => {
    const last6ReportPage = Last6ReportPage(page);
    await last6ReportPage.goTo()

    const actualMembers = await last6ReportPage.getLast6ScannedMembers()
    expect(actualMembers.length).toBe(last6ScannedMembers.length)

    for (let i = 0; i < last6ScannedMembers.length; i++) {
        const expectedMember = last6ScannedMembers[i];
        const actualMember = actualMembers[i];
        expect(actualMember).toEqual(expect.objectContaining(expectedMember));
    }
}

export {last6ScannersAssertionHelpers}