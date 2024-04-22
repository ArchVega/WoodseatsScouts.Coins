const VoteSummaryPage = (page) => {
    return {
        getThanksForVotingMessage: async () => {
            return await page.getByTestId("thanks-for-voting-message").textContent()
        }
    }
}

export default VoteSummaryPage;