import {expect} from "@playwright/test";

const MembersPage = (page) => {
    let membersTable = null;

    return {
        goTo: async () => {
            await page.goto('/members');

            membersTable = page.getByTestId("table-members")

            await expect(membersTable).toBeVisible()
        },

        getAllMembers: async (includeEditPhotoLink) => {
            let trs = await page.locator("tr").all()
            const results = [];

            for (const tr of trs) {
                const tds = await tr.locator("td").all();

                if (tds.length > 0) {
                    const row = {
                        name: await tds[1].textContent(),
                        group: await tds[2].textContent(),
                        section: await tds[3].textContent(),
                        totalPoints: Number(await tds[4].textContent()),
                        wristCode: await tds[5].textContent(),
                    };

                    if (includeEditPhotoLink) {
                        row.editPhotoTestId = await tds[6].locator('span').getAttribute("data-testid")
                    }

                    results.push(row);
                }
            }

            return results;
        },

        async showTotalPoints(show) {
            await page.getByTestId("checkbox-show-total-points").setChecked(show)
        },

        async searchMembers(searchText) {
            const membersTextbox = await page.getByTestId("textbox-search-members");
            await membersTextbox.fill(searchText);
            await page.waitForTimeout(500)

            return this.getAllMembers();
        }
    }
}

export default MembersPage