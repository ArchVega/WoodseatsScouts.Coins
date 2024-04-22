const MemberScavengedResultPage = (page) => {
    return {
        getTotalPoints: async () => {
            const spanTotalPointsSaved = page.getByTestId("coin-total");

            const result = await spanTotalPointsSaved.textContent()

            return Number(result);
        },

        async getAdditionalMessage() {
            const spanTotalPointsSaved = page.getByTestId("div-additional-message");

            return await spanTotalPointsSaved.textContent()
        }
    }
}

export default MemberScavengedResultPage;