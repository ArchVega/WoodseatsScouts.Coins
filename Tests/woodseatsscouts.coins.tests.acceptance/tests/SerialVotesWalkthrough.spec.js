// todo: There are several more test cases in Todo.txt to implement.
import {test, expect} from '@playwright/test';
import MembersPage from "../pageModels/MembersPage";
import Helpers from "../Helpers";
import ScavengerHunt from "../models/ScavengerHunt";
import Users from "../models/Users";
import ScreenshotsComparer from "../utilities/ScreenshotsComparer";
import VotePage from "../pageModels/VotePage";
import VotesLeaderboardPage from "../pageModels/VoteLeaderboardPage";
import exp from "node:constants";
import ToastMessageModel from "../uiModels/ToastMessageModel";
import axios from "axios";
import Uris from "../pageModels/Uris";

let runName = "master"
runName = "feature"

let screenshotsComparer = null;
let users = Users();
let scavengerHunt;
(async () => {
    scavengerHunt = await ScavengerHunt()
})();
let stepCounter = 1;

function serialStep(name) {
    const counter = ('00' + stepCounter++).substring(-2)
    return `Step ${counter}: ${name}`
}

test(serialStep("Creating users"), async ({page}, testInfo) => {
    // screenshotsComparer =  ScreenshotsComparer("screenshots", runName); this deletes the existing master folder
    screenshotsComparer = ScreenshotsComparer("screenshots"); // this doesn't delete the folder

    Helpers().restoreDb()

    const membersPage = MembersPage(page);
    await membersPage.goTo()

    await Helpers().createTroopsViaApi([
        {id: 1, name: "Crimson"},
        {id: 2, name: "Jet"},
        {id: 3, name: "Royal"},
        {id: 4, name: "Saffron"},
    ])

    await Helpers().createMembersViaApi([
        {
            firstNames: ["Charcoal", "Icterine", "Olivine", "Turquoise"],
            lastName: "Crimson",
            troopId: 1,
            section: "A",
            isDayVisitor: false
        },
        {
            firstNames: ["Glaucous", "Pistachio", "Pumpkin", "Red"],
            lastName: "Jet",
            troopId: 2,
            section: "B",
            isDayVisitor: false
        },
        {
            firstNames: ["Asparagus", "Cerise", "Ghost", "Jasper"],
            lastName: "Royal",
            troopId: 3,
            section: "C",
            isDayVisitor: false
        },
        {
            firstNames: ["Hunter", "Oxford", "Rosewood", "Violet"],
            lastName: "Saffron",
            troopId: 4,
            section: "E",
            isDayVisitor: false
        },
    ]);

    await page.waitForTimeout(3000);
    await membersPage.goTo()

    const members = await membersPage.getAllMembers();
    expect(members.length).toBe(16);
    expect(members[0].name).toBe("Asparagus Royal")
    expect(members[15].name).toBe("Violet Saffron")

    await Helpers().copyTestMemberImages()

    await screenshotsComparer.takeScreenshot(page, testInfo, "End");
});

test(serialStep("Asparagus votes for Poland"), async ({page}, testInfo) => {
    const votePage = VotePage(page);
    await votePage.goTo()

    const voteForCountryPage = await votePage.simulateValidUserWristbandScan(users.asparagusRoyal)
    await voteForCountryPage.selectCountry("poland")

    const voteSummaryPage = await voteForCountryPage.confirmVoteForCountry();
    const thanksForVotingMessage = await voteSummaryPage.getThanksForVotingMessage();
    const countryVotedForMessage = await voteSummaryPage.getCountryVotedForMessage();
    expect(thanksForVotingMessage).toBe("Thanks for voting Asparagus👍")
    expect(countryVotedForMessage).toBe("You've voted for Poland!")
});

test(serialStep("Votes leaderboard page is as expected"), async ({page}, testInfo) => {
    const votesLeaderboardPage = VotesLeaderboardPage(page);
    await votesLeaderboardPage.goTo()

    const expectation = [
        { count: 1, countryName: "Poland"},
        { count: 0, countryName: "Australia"},
        { count: 0, countryName: "Belgium"},
        { count: 0, countryName: "Finland"},
        { count: 0, countryName: "France"},
        { count: 0, countryName: "Germany"},
        { count: 0, countryName: "Ireland"},
        { count: 0, countryName: "Italy"},
        { count: 0, countryName: "Norway"},
        { count: 0, countryName: "Spain"},
    ]
    await votesLeaderboardPage.verifyVotes(expectation)
})

test(serialStep("Icterine votes for Australia"), async ({page}, testInfo) => {
    const votePage = VotePage(page);
    await votePage.goTo()

    const voteForCountryPage = await votePage.simulateValidUserWristbandScan(users.icterineCrimson)
    await voteForCountryPage.selectCountry("Australia")

    const voteSummaryPage = await voteForCountryPage.confirmVoteForCountry();
    const thanksForVotingMessage = await voteSummaryPage.getThanksForVotingMessage();
    const countryVotedForMessage = await voteSummaryPage.getCountryVotedForMessage();
    expect(thanksForVotingMessage).toBe("Thanks for voting Icterine👍")
    expect(countryVotedForMessage).toBe("You've voted for Australia!")
});

test(serialStep("Votes leaderboard page is as expected"), async ({page}, testInfo) => {
    const votesLeaderboardPage = VotesLeaderboardPage(page);
    await votesLeaderboardPage.goTo()

    const expectation = [
        { count: 1, countryName: "Australia"},
        { count: 1, countryName: "Poland"},
        { count: 0, countryName: "Belgium"},
        { count: 0, countryName: "Finland"},
        { count: 0, countryName: "France"},
        { count: 0, countryName: "Germany"},
        { count: 0, countryName: "Ireland"},
        { count: 0, countryName: "Italy"},
        { count: 0, countryName: "Norway"},
        { count: 0, countryName: "Spain"},
    ]
    await votesLeaderboardPage.verifyVotes(expectation)
})

test(serialStep("Turquoise votes for Finland"), async ({page}, testInfo) => {
    const votePage = VotePage(page);
    await votePage.goTo()

    const voteForCountryPage = await votePage.simulateValidUserWristbandScan(users.turquoiseCrimson)
    await voteForCountryPage.selectCountry("Finland")

    const voteSummaryPage = await voteForCountryPage.confirmVoteForCountry();
    const thanksForVotingMessage = await voteSummaryPage.getThanksForVotingMessage();
    const countryVotedForMessage = await voteSummaryPage.getCountryVotedForMessage();
    expect(thanksForVotingMessage).toBe("Thanks for voting Turquoise👍")
    expect(countryVotedForMessage).toBe("You've voted for Finland!")
});

test(serialStep("Votes leaderboard page is as expected"), async ({page}, testInfo) => {
    const votesLeaderboardPage = VotesLeaderboardPage(page);
    await votesLeaderboardPage.goTo()

    const expectation = [
        { count: 1, countryName: "Australia"},
        { count: 1, countryName: "Finland"},
        { count: 1, countryName: "Poland"},
        { count: 0, countryName: "Belgium"},
        { count: 0, countryName: "France"},
        { count: 0, countryName: "Germany"},
        { count: 0, countryName: "Ireland"},
        { count: 0, countryName: "Italy"},
        { count: 0, countryName: "Norway"},
        { count: 0, countryName: "Spain"},
    ]
    await votesLeaderboardPage.verifyVotes(expectation)
})

test(serialStep("Icterine tries voting again for Australia"), async ({page}, testInfo) => {
    const votePage = VotePage(page);
    await votePage.goTo()

    const membersResponse = await axios.get(Uris.sutMembers)
    const userData = membersResponse.data.filter(member => (member.firstName + " " + member.lastName) === users.icterineCrimson)[0]
    const memberCodeTextbox = page.getByTestId("textbox-usb-scanner-code")
    await memberCodeTextbox.fill(userData.code);
    await memberCodeTextbox.press('Enter')

    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe("Icterine has already casted their vote!")
});