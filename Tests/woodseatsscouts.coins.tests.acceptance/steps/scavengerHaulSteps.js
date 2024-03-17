import HomePage from "../pageModels/HomePage";
import {expect} from "@playwright/test";

const validScavengerHaulSteps = async (user, scavengedCoins, expectedTotal, page) => {
    const homePage = HomePage(page);
    await homePage.goTo()

    const coinCodeScanPage = await homePage.simulateValidUserWristbandScan(user)

    for (let i = 0; i < scavengedCoins.length; i++) {
        let coin = scavengedCoins[i]
        await coinCodeScanPage.enterCoinCode(coin.code)
    }

    const scannedCoins = await coinCodeScanPage.getScannedCoins();
    const scannedCoinsTotal = scannedCoins.reduce((partialSum, a) => partialSum + a, 0);
    const expectedCoinsTotal = scavengedCoins.map(x => x.value).reduce((partialSum, a) => partialSum + a, 0);
    expect(scannedCoins.length).toEqual(scavengedCoins.length)
    expect(scannedCoinsTotal).toEqual(expectedCoinsTotal)

    const totalOnPage = Number(await page.getByTestId("coin-total").textContent())
    expect(totalOnPage).toBe(expectedCoinsTotal);
    const memberScavengedResultPage = await coinCodeScanPage.clickFinishScanningButton()

    expect(await memberScavengedResultPage.getTotalPoints()).toBe(expectedTotal)

    return memberScavengedResultPage;
}

const scansAllCoinsButDoesNotClickFinish = async (user, scavengedCoins, expectedTotal, page) => {
    const homePage = HomePage(page);
    await homePage.goTo()

    const coinCodeScanPage = await homePage.simulateValidUserWristbandScan(user)

    for (let i = 0; i < scavengedCoins.length; i++) {
        let coin = scavengedCoins[i]
        await coinCodeScanPage.enterCoinCode(coin.code)
    }

    const scannedCoins = await coinCodeScanPage.getScannedCoins();
    const scannedCoinsTotal = scannedCoins.reduce((partialSum, a) => partialSum + a, 0);
    const expectedCoinsTotal = scavengedCoins.map(x => x.value).reduce((partialSum, a) => partialSum + a, 0);
    expect(scannedCoins.length).toEqual(scavengedCoins.length)
    expect(scannedCoinsTotal).toEqual(expectedCoinsTotal)

    const totalOnPage = Number(await page.getByTestId("coin-total").textContent())
    expect(totalOnPage).toBe(expectedCoinsTotal);

    return coinCodeScanPage;
}

const clicksFinishButtonWithExpectation = async (coinCodeScanPage, expectedTotal) => {
    const memberScavengedResultPage
        = await coinCodeScanPage.clickFinishScanningButton()
    expect(await memberScavengedResultPage.getTotalPoints()).toBe(expectedTotal)

    return memberScavengedResultPage;
}

export {validScavengerHaulSteps, scansAllCoinsButDoesNotClickFinish, clicksFinishButtonWithExpectation}