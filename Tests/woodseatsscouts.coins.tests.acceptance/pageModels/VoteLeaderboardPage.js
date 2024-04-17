import {expect} from "@playwright/test";
import axios from "axios";
import Uris from "./Uris";
import VoteForCountryPage from "./VoteForCountryPage";

const VoteLeaderboardPage = (page) => {
    return {
        goTo: async () => {
            await page.goto('/vote-results');
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

        async verifyVotes(expectedVotes) {
            if (expectedVotes.length !== 10) {
                throw "Expected votes length is not 10"
            }

            const leaderboardItems = await page.locator(".vote-leaderboard-item").all()

            for (let i = 0; i < leaderboardItems.length; i++) {
                const actualItemTotalVotesCount = Number(await leaderboardItems[i].textContent());
                const actualItemCountryName = await leaderboardItems[i].getAttribute("data-country")

                const expectedVote = expectedVotes[i]
                expect(expectedVote.count).toBe(actualItemTotalVotesCount)
                expect(expectedVote.countryName).toBe(actualItemCountryName)
            }
        }
    }
}

export default VoteLeaderboardPage