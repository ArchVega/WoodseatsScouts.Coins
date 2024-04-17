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

test.use({
    launchOptions: {
        slowMo: 50
    }
});

// Todo: requires Creating users to be run first. That test should be moved into a shared fixture.
test(
    "Turquoise Crimson completes an enormous haul of 50 coins and inputs coins blisteringly fast",
    async ({page}) => {
        const coinValues = Array(10).fill([3, 9, 11, 10, 20]).flat()
        const coins = await scavengerHunt.getUnscavengedCoinByValue(users.turquoiseCrimson, coinValues)
        await validScavengerHaulSteps(users.turquoiseCrimson, coins, 530, page)
    });
