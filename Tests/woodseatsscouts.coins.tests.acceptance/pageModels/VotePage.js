import {expect} from "@playwright/test";
import axios from "axios";
import CoinCodeScanPage from "./CoinCodeScanPage";
import Uris from "./Uris";
import VoteForCountryPage from "./VoteForCountryPage";

const VotePage = (page) => {
    return {
        goTo: async () => {
            await page.goto('/vote');
        },

        simulateValidUserWristbandScan: async (userName) => {
            const membersResponse = await axios.get(Uris.sutMembers)
            const userData = membersResponse.data.filter(member => (member.firstName + " " + member.lastName) === userName)[0]
            const memberCodeTextbox = page.getByTestId("textbox-usb-scanner-code")
            await memberCodeTextbox.fill(userData.code);
            await memberCodeTextbox.press('Enter')

            const userFirstName = await page.getByTestId("h1-user-firstname").textContent()
            expect(userFirstName).toBe(userData.firstName)

            return VoteForCountryPage(page);
        },
    }
}

export default VotePage