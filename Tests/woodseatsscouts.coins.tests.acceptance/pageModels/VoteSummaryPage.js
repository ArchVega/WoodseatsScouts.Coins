const VoteSummaryPage = (page) => {
    return {
        getThanksForVotingMessage: async () => {
            return await page.getByTestId("thanks-for-voting-message").textContent()
        },

        getCountryVotedForMessage: async () => {
            return await page.getByTestId("country-voted-for-message").textContent()
        },
    }
}

export default VoteSummaryPage;