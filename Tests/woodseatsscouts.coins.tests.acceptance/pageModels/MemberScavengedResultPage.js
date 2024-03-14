const MemberScavengedResultPage = (page) => {
    return {
        getTotalPoints: async () => {
            const spanTotalPointsSaved = page.getByTestId("span-total-points-saved");

            const result = await spanTotalPointsSaved.textContent()

            return Number(result);
        }
    }
}

export default MemberScavengedResultPage;