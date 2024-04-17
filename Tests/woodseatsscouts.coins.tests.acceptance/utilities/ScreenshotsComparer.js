import path from "node:path";
import joinImages from "join-images";

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

    // Todo: this is some bad logic.
    let runDirectory = path.join(screenshotsDirectory, "comparisons");

    if (runName !== undefined) {
        // "Run Directories" should be deleted before each test run.
        runDirectory = path.join(screenshotsDirectory, runName);
        deleteDirectoryIfExists(runDirectory)
        createDirectoryIfNotExists(runDirectory);
    }

    const names = []

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
            const stepNameIndex = names.filter(x => x === name).length
            names.push(name)
            name = `${name}-${stepNameIndex}-${description}.png`
            const filePath = path.join(directory, name);
            await page.screenshot({ path: filePath, fullPage: true });
        },

        createComparisons: function(runName, testInfo) {
            if (runName.toLowerCase() === "master") {
                throw "RunName is master, change to feature"
            }

            const comparisonsDirectory = path.join(screenshotsDirectory, "comparisons");
            deleteDirectoryIfExists(comparisonsDirectory)
            createDirectoryIfNotExists(comparisonsDirectory);

            let directory = testInfo.titlePath[0].replace(/\.spec\.js/g, '').replace(/\./g, '_');
            const masterFiles =fs.readdirSync(`screenshots/master/${directory}`)

            masterFiles.forEach(masterFile => {
                const before = `screenshots/master/SerialWalkthrough/${masterFile}`
                const after = `screenshots/feature/SerialWalkthrough/${masterFile}`
                joinImages([before, after], { direction: "horizontal"}).then((img) => {
                    const outFile = path.join(comparisonsDirectory, masterFile)
                    img.toFile(outFile).then(r => {});
                });
            })
        }
    }
}

export default ScreenshotsComparer