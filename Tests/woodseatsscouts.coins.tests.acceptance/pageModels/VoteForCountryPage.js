import MemberScavengedResultPage from './MemberScavengedResultPage'
import VoteSummaryPage from "./VoteSummaryPage";

const VoteForCountryPage = (page) => {
    return {
        selectCountry: async (countryName) => {
            await page.getByTestId(`vote-for-${countryName.toLowerCase()}`).click()
        },

        confirmVoteForCountry: async () => {
            await page.getByTestId("confirm-vote-for-country").click()

            return VoteSummaryPage(page)
        }
    }
}

export default VoteForCountryPage