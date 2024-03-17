import path from "node:path";

const fs = require('node:fs');

function ScreenshotsComparer(screenshotsDirectory, runName) {
    function deleteDirectoryIfExists(dirName) {
        try {
            if (fs.existsSync(dirName)) {
                fs.rmSync(dirName, { recursive: true, force: true });
            }
        } catch (err) {
            console.error(err);
        }
    }

    function createDirectoryIfNotExists(dirName) {
        try {
            if (!fs.existsSync(dirName)) {
                fs.mkdirSync(dirName, {recursive: true});
            }
        } catch (err) {
            console.error(err);
        }
    }

    // Creates the directory for the first time if it doesn't exist. This directory should NOT be deleted afterwards.
    createDirectoryIfNotExists(screenshotsDirectory);

    // "Run Directories" should be deleted before each test run.
    const runDirectory = path.join(screenshotsDirectory, runName);
    deleteDirectoryIfExists(runDirectory)
    createDirectoryIfNotExists(runDirectory);

    return {
        takeScreenshot: async (page, testInfo, description) => {
            if (description === undefined || description === null || description.length === 0) {
                const message = `Screenshot for test ${testInfo.title} cannot be null or empty`
                console.log(message)
                throw message
            }

            let directory = testInfo.titlePath[0].replace(/\.spec\.js/g, '').replace(/\./g, '_');
            directory = path.join(runDirectory, directory);
            if (!fs.existsSync(directory)) {
                fs.mkdirSync(directory);
            }

            let name = testInfo.titlePath[1].replace(/:/g,'-').replace(/\./g, '_');
            name = name + ".png"
            const filePath = path.join(directory, name);
            await page.screenshot({ path: filePath, fullPage: true });
        }
    }
}

export default ScreenshotsComparer