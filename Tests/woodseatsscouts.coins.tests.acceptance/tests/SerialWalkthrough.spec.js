// todo: There are several more test cases in Todo.txt to implement.
import {test, expect} from '@playwright/test';
import MembersPage from "../pageModels/MembersPage";
import Helpers from "../Helpers";
import ScavengerHunt from "../models/ScavengerHunt";
import Users from "../models/Users";
import {assertReportsPageIs} from "../assertionHelpers/ReportsPageAssertionHelpers";
import {
    clicksFinishButtonWithExpectation,
    scansAllCoinsButDoesNotClickFinish,
    validScavengerHaulSteps
} from "../steps/scavengerHaulSteps";
import HomePage from "../pageModels/HomePage";
import ToastMessageModel from "../uiModels/ToastMessageModel";
import {assertMembersPageContains} from "../assertionHelpers/MembersPageAssertionHelpers";
import ReportPage from "../pageModels/ReportPage";

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

test(serialStep("Creating users"), async ({page}) => {
    await Helpers().setDeadlineTime(2)

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

    // Page loads too fast for the data?
    await page.waitForTimeout(3000);
    await membersPage.goTo()

    const members = await membersPage.getAllMembers();
    expect(members.length).toBe(16);
    expect(members[0].name).toBe("Asparagus Royal")
    expect(members[15].name).toBe("Violet Saffron")

    await Helpers().copyTestMemberImages()
});

test(serialStep("Testing search"), async ({page}) => {
    const membersPage = MembersPage(page);
    await membersPage.goTo()

    let results = await membersPage.searchMembers("Asparagus")
    expect(results.length).toBe(1)
    expect(results[0].name).toBe("Asparagus Royal")

    results = await membersPage.searchMembers("saff")
    expect(results.length).toBe(4)
    expect(results.map(x => x.name)).toEqual(expect.arrayContaining(["Hunter Saffron", "Oxford Saffron", "Rosewood Saffron", "Violet Saffron"]))

    results = await membersPage.searchMembers("Beavers")
    expect(results.length).toBe(4)
    expect(results.map(x => x.name)).toEqual(expect.arrayContaining(["Glaucous Jet", "Pistachio Jet", "Pumpkin Jet", "Red Jet"]))

    results = await membersPage.searchMembers("M003C003")
    expect(results.length).toBe(1)
});

test(serialStep("Testing navigation"), async ({page}) => {
    const homePage = HomePage(page);
    await homePage.goTo()

    await page.getByTestId("nav-coins-page").click();
    await expect(page).toHaveURL("")

    await page.getByTestId("nav-members-page").click();
    await expect(page).toHaveURL("members")

    await page.getByTestId("nav-report-page").click();
    await expect(page).toHaveURL("leaderboard")

    await page.getByTestId("nav-settings-modal").click();
    const useCameraSwitch = page.getByTestId("switch-use-camera");
    await expect(useCameraSwitch).toBeVisible()
});

test(serialStep("Edit photo modal appears for a user"), async ({page}) => {
    const membersPage = MembersPage(page);
    await membersPage.goTo()

    const anyMember = (await membersPage.getAllMembers())[0]
    await page.getByTestId(anyMember.editPhotoTestId).click()
    const button = await page.getByText("Capture photo");
    await expect(button).toBeVisible();
});

test(serialStep("Invalid wrist code shows an error toast message"), async ({page}) => {
    const homePage = HomePage(page);
    await homePage.goTo()
    await homePage.simulateInvalidUserWristbandScan("member-code-does-not-exist")

    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe("Could not translate Member Code 'member-code-does-not-exist'")
});

test(serialStep("Scanning a coin instead of a wristband for logging in a member shows an error toast message"), async ({page}) => {
    const homePage = HomePage(page);
    await homePage.goTo()
    const anyCoinCode = await scavengerHunt.peekUnscavengedCoin(20);
    await homePage.simulateInvalidUserWristbandScan(anyCoinCode.code)

    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe(`The code '${anyCoinCode.code}' is a Coin code`)
});

test(serialStep("User presses the Finished Scanning button for Asparagus Royal without any coins added"), async ({page}) => {
    const homePage = HomePage(page);
    await homePage.goTo()

    const coinCodeScanPage = await homePage.simulateValidUserWristbandScan(users.asparagusRoyal)

    await coinCodeScanPage.clickFinishScanningButton();

    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe("Add at least one coin for this member")
});

test(serialStep("Invalid coin code for Asparagus Royal shows an error toast message"), async ({page}) => {
    const homePage = HomePage(page);
    await homePage.goTo()

    const coinCodeScanPage = await homePage.simulateValidUserWristbandScan(users.asparagusRoyal)

    await coinCodeScanPage.enterCoinCode("coin-code-does-not-exist")

    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe("Could not translate Coin Code 'coin-code-does-not-exist'")
});

test(serialStep("Scanning a wristband instead of a coin when logging coins shows an error toast message"), async ({page}) => {
    const homePage = HomePage(page);
    await homePage.goTo()

    const coinCodeScanPage = await homePage.simulateValidUserWristbandScan(users.violetSaffron)

    const anyMemberCode = "M001A003"
    await coinCodeScanPage.enterCoinCode(anyMemberCode)

    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe(`The code '${anyMemberCode}' is a Member code`)
});

test(serialStep("Asparagus Royal completes a scavenger haul"), async ({page}) => {
    const coins = await scavengerHunt.getUnscavengedCoinByValue(users.asparagusRoyal, [10, 3, 20])
    await validScavengerHaulSteps(users.asparagusRoyal, coins, 33, page)
});

test(serialStep("Viewing the report page"), async ({page}) => {
    await assertReportsPageIs(
        page,
        [
            {userName: users.asparagusRoyal, troopName: "Royal", sectionName: "Cubs", userPoints: 33}
        ],
        [
            {groupName: "Royal", averagePoints: 8.25}
        ],
        [
            {groupName: "Royal", averagePoints: 8.25}
        ]
    )
});

test(serialStep("Icterine Crimson completes a scavenger haul"), async ({page}) => {
    const coins = await scavengerHunt.getUnscavengedCoinByValue(users.icterineCrimson, [20, 20])
    await validScavengerHaulSteps(users.icterineCrimson, coins, 40, page)
});

test(serialStep("Viewing the report page"), async ({page}) => {
    await assertReportsPageIs(
        page,
        [
            {userName: users.icterineCrimson, troopName: "Crimson", sectionName: "Adults", userPoints: 40},
            {userName: users.asparagusRoyal, troopName: "Royal", sectionName: "Cubs", userPoints: 33}
        ],
        [
            {groupName: "Crimson", averagePoints: 10},
            {groupName: "Royal", averagePoints: 8.25}
        ],
        [
            {groupName: "Crimson", averagePoints: 10},
            {groupName: "Royal", averagePoints: 8.25}
        ]
    )
});

// test(serialStep("(user) cancels a haul returns the assigned scavenged coins back into circulation"), async ({page}) => {
//     // Todo: hold the coins in memory so they're not added at all until done? or create an admin page?
//     // const coins = await scavengerHunt.getUnscavengedCoinByValue(users.icterineCrimson, [20, 20])
//     // await validScavengerHaulSteps(users.icterineCrimson, coins, 40, page)
// });

test(serialStep("Cerise Royal completes a scavenger haul"), async ({page}) => {
    const coins = await scavengerHunt.getUnscavengedCoinByValue(users.ceriseRoyal, [10, 20])
    await validScavengerHaulSteps(users.ceriseRoyal, coins, 30, page)
});

test(serialStep("Viewing the report page"), async ({page}) => {
    await assertReportsPageIs(
        page,
        [
            {userName: users.ceriseRoyal, troopName: "Royal", sectionName: "Cubs", userPoints: 30},
            {userName: users.icterineCrimson, troopName: "Crimson", sectionName: "Adults", userPoints: 40},
            {userName: users.asparagusRoyal, troopName: "Royal", sectionName: "Cubs", userPoints: 33},
        ],
        [
            {groupName: "Royal", averagePoints: 15.75},
            {groupName: "Crimson", averagePoints: 10}
        ],
        [
            {groupName: "Royal", averagePoints: 15.75},
            {groupName: "Crimson", averagePoints: 10}
        ]
    )
});

test(serialStep("Glaucous Jet completes a scavenger haul"), async ({page}) => {
    const coins = await scavengerHunt.getUnscavengedCoinByValue(users.glaucousJet, [20, 10, 9, 10])
    await validScavengerHaulSteps(users.glaucousJet, coins, 49, page)
});

test(serialStep("Viewing the report page"), async ({page}) => {
    await assertReportsPageIs(
        page,
        [
            {userName: users.glaucousJet, troopName: "Jet", sectionName: "Beavers", userPoints: 49},
            {userName: users.ceriseRoyal, troopName: "Royal", sectionName: "Cubs", userPoints: 30},
            {userName: users.icterineCrimson, troopName: "Crimson", sectionName: "Adults", userPoints: 40},
        ],
        [
            {groupName: "Royal", averagePoints: 15.75},
            {groupName: "Jet", averagePoints: 12.25},
            {groupName: "Crimson", averagePoints: 10},
        ],
        [
            {groupName: "Royal", averagePoints: 15.75},
            {groupName: "Jet", averagePoints: 12.25},
            {groupName: "Crimson", averagePoints: 10},
        ]
    )
});

// test(serialStep("Viewing the report page"), async ({page}) => {
//     await assertReportsPageIs(
//         page,
//         [
//             {userName: users.glaucousJet, troopName: "Jet", sectionName: "Beavers", userPoints: 49},
//             {userName: users.ceriseRoyal, troopName: "Royal", sectionName: "Cubs", userPoints: 30},
//             {userName: users.icterineCrimson, troopName: "Crimson", sectionName: "Adults", userPoints: 40},
//         ],
//         [
//             {groupName: "Royal", averagePoints: 15.75},
//             {groupName: "Jet", averagePoints: 12.25},
//             {groupName: "Crimson", averagePoints: 10},
//         ],
//         [
//             {groupName: "Royal", averagePoints: 15.75},
//             {groupName: "Jet", averagePoints: 12.25},
//             {groupName: "Crimson", averagePoints: 10},
//         ]
//     )
// });

test(serialStep("Viewing the members page"), async ({page}) => {
    await assertMembersPageContains(
        page,
        [
            {name: users.asparagusRoyal, group: "Royal", totalPoints: 33, section: "Cubs"},
            {name: users.ceriseRoyal, group: "Royal", totalPoints: 30, section: "Cubs"},
            {name: users.charcoalCrimson, group: "Crimson", totalPoints: 0, section: "Adults"},
            {name: users.glaucousJet, group: "Jet", totalPoints: 49, section: "Beavers"},
            {name: users.icterineCrimson, group: "Crimson", totalPoints: 40, section: "Adults"},
        ]
    )
});

test(serialStep("Olivine Crimson tries to use someone else's coin"), async ({page}) => {
    const coin1 = (await scavengerHunt.getUnscavengedCoinByValue(users.olivineCrimson, [20]))[0]
    const coin2 = (await scavengerHunt.getAnAlreadyScavengedCoin(users.olivineCrimson))
    const coin3 = (await scavengerHunt.getUnscavengedCoinByValue(users.olivineCrimson, [20]))[0]

    const user = users.olivineCrimson
    const homePage = HomePage(page);
    await homePage.goTo()

    const coinCodeScanPage = await homePage.simulateValidUserWristbandScan(user)

    await coinCodeScanPage.enterCoinCode(coin1.code)
    expect(await coinCodeScanPage.getTotalCoinValue()).toBe(20)

    await coinCodeScanPage.enterCoinCode(coin2.code)
    const errorMessage = await ToastMessageModel(page).getMessage()
    expect(errorMessage).toBe(`The coin with code '${coin2.code}' has already been scavenged by ${coin2.fullName}!`)
    expect(await coinCodeScanPage.getTotalCoinValue()).toBe(20)

    await coinCodeScanPage.enterCoinCode(coin3.code)
    expect(await coinCodeScanPage.getTotalCoinValue()).toBe(40)
});

test(
    serialStep("Jasper Royal and Oxford Saffron try to scan the same coin for each of their hauls"),
    async ({context, page}) => {
        const jasperPage = {
            member: users.jasperRoyal,
            page: page,
            coins: []
        };
        const oxfordPage = {
            member: users.oxfordSaffron,
            page: await context.newPage(),
            coins: []
        }

        const memberPages = [jasperPage, oxfordPage];

        const specificCoinToBeShared = await scavengerHunt.peekUnscavengedCoin(20);

        for (const memberPage of memberPages) {
            const coins = await scavengerHunt.getUnscavengedCoinByValue(memberPage.member, [3, 10])
            coins.push(specificCoinToBeShared)
            memberPage.coins = coins;
        }

        // First Jasper scans his coins
        const jasperCoinPage = await scansAllCoinsButDoesNotClickFinish(jasperPage.member, jasperPage.coins, 33, jasperPage.page)

        // Next Oxford scans his coins, including the shared coin
        const oxfordCoinPage = await scansAllCoinsButDoesNotClickFinish(oxfordPage.member, oxfordPage.coins, 33, oxfordPage.page)

        // Now Jasper clicks the Finish button (we also mark the in-memory coin as allocated to him so other tests won't use it)
        await clicksFinishButtonWithExpectation(jasperCoinPage, 33)
        scavengerHunt.allocateTestCoinToUser(specificCoinToBeShared, Users.jasperRoyal)

        // Now Oxford tries, only two coins are recorded, and the results page should show an additional message
        const oxfordScavengedResultPage
            = await clicksFinishButtonWithExpectation(oxfordCoinPage, 13)
        const additionalMessage = await oxfordScavengedResultPage.getAdditionalMessage();
        expect(additionalMessage)
            . toEqual(`Unfortunately, there was an issue with at least one of your coins.${specificCoinToBeShared.code} scanned by Jasper Royal`)
    });

test(serialStep("Time's up"), async ({page}) => {
    const reportPage = ReportPage(page);
    await reportPage.goTo()

    let hoursLeft = await reportPage.getHoursLeft()
    expect(hoursLeft).toBeGreaterThan(0);

    await Helpers().setDeadlineTime(-1)
    await reportPage.goTo()

    hoursLeft = await reportPage.getHoursLeft()
    expect(hoursLeft).toBe(0)
});