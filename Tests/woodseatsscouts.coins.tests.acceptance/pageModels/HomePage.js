import {expect} from "@playwright/test";
import axios from "axios";
import CoinCodeScanPage from "./CoinCodeScanPage";
import Uris from "./Uris";

const HomePage = (page) => {
    return {
        goTo: async () => {
            await page.goto('');

            await expect(page).toHaveTitle(/Woodseats Scouts Coins/);
        },

        simulateValidUserWristbandScan: async (userName) => {
            const membersResponse = await axios.get(Uris.sutMembers)
            const userData = membersResponse.data.filter(member => (member.firstName + " " + member.lastName) === userName)[0]
            const memberCodeTextbox = page.getByTestId("textbox-usb-scanner-code")
            await memberCodeTextbox.fill(userData.code);
            await memberCodeTextbox.press('Enter')

            const userFirstName = await page.getByTestId("h1-user-firstname").textContent()
            expect(userFirstName).toBe(userData.firstName)

            return CoinCodeScanPage(page);
        },

        simulateInvalidUserWristbandScan: async (wristCode) => {
            const memberCodeTextbox = page.getByTestId("textbox-usb-scanner-code")
            await memberCodeTextbox.fill(wristCode);
            await memberCodeTextbox.press('Enter')

            return CoinCodeScanPage(page);
        },


    }
}

export default HomePage