import axios from "axios";
import * as fs from "fs";
import Uris from "./pageModels/Uris";

const Helpers = () => {
    return {
        restoreDb: () => {
            const currentDirectory = process.cwd()

            try {
                process.chdir("../..");
                const child = require("child_process").execSync("pwsh ./Utilities/Database/RecreateDbs-3-AcceptanceTests.ps1");
                console.info(child.toString('utf8'))


            } finally {
                process.chdir(currentDirectory);
            }
        },

        setSystemDateTime: async (minutesToAdd) => {
            throw "obsoleted (temporarily?)"
        },
        // setSystemDateTime: async (minutesToAdd) => {
        //     await axios({
        //         url: Uris.sutSystemDateTime(minutesToAdd),
        //         method: 'PUT',
        //         headers: {
        //             'X-Coins-Authentication-Token': 'test',
        //             'Content-Type': 'application/json'
        //         }
        //     }).then(response => {
        //         console.log(response)
        //     }).catch(reason => {
        //         console.error(reason)
        //     })
        // },

        copyTestMemberImages: async () => {
            const membersResponse = await axios.get(Uris.sutMembers)

            let promises = []
            membersResponse.data.forEach(member => {
                const source = member.lastName + "-" + member.firstName + ".png";
                const photoContent = fs.readFileSync(`./testImages/${source}`)
                const photoBytes = Buffer.from(photoContent).toString('base64')

                const promise = axios({
                    url: Uris.memberPhoto(member),
                    method: 'PUT',
                    headers: {
                        'X-Coins-Authentication-Token': 'test',
                        'Content-Type': 'application/json'
                    },
                    data: {
                        Photo: photoBytes
                    }
                })
                promises.push(promise)
            })

            await Promise.all(promises)
                .then(response => console.log(response))
                .catch(error => console.error(error))
            const i = 0;
        },

        createMembersViaApi: async (users) => {
            users.forEach(user => {
                user.firstNames.forEach(async firstName => {
                    await axios({
                        url: Uris.adminCreateMember,
                        method: 'POST',
                        headers: {
                            'X-Coins-Authentication-Token': 'test',
                            'Content-Type': 'application/json'
                        },
                        data: {
                            firstName: firstName,
                            lastName: user.lastName,
                            scoutGroupId: user.scoutGroupId,
                            section: user.section,
                            isDayVisitor: user.isDayVisitor
                        }
                    }).then(response => {
                        console.log(response)
                    }).catch(reason => {
                        console.error(reason)
                    })
                });
            })
        },

        createScoutGroupsViaApi:
            async (scoutGroups) => {
                for (const scoutGroup of scoutGroups) {
                    await axios({
                        url: Uris.adminCreateScoutGroup,
                        method: 'POST',
                        headers: {
                            'X-Coins-Authentication-Token': 'test',
                            'Content-Type': 'application/json'
                        },
                        data: {
                            id: scoutGroup.id,
                            name: scoutGroup.name
                        }
                    }).then(response => {
                        console.log(response)
                    }).catch(reason => {
                        console.error(reason)
                    })
                }
            }
    }
}

export default Helpers