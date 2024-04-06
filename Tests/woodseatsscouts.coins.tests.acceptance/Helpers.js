import axios from "axios";
import * as fs from "fs";
import Uris from "./pageModels/Uris";

const Helpers = () => {
    return {
        restoreDb: () => {
            const currentDirectory = process.cwd()

            try {
                process.chdir("..\\..");
                const child = require("child_process").execSync("pwsh.exe .\\Utilities\\Database\\RecreateDbs-3-AcceptanceTests.ps1");
                console.info(child.toString('utf8'))


            } finally {
                process.chdir(currentDirectory);
            }
        },

        setSystemDateTime: async (minutesToAdd) => {
            await axios({
                url: Uris.sutSystemDateTime(minutesToAdd),
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(response => {
                console.log(response)
            }).catch(reason => {
                console.error(reason)
            })
        },

        setDeadlineTime: async (minutesToAdd) => {
            await axios({
                url: Uris.sutScavengerHuntDeadline(minutesToAdd),
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(response => {
                console.log(response)
            }).catch(reason => {
                console.error(reason)
            })
        },

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
                            'Content-Type': 'application/json'
                        },
                        data: {
                            firstName: firstName,
                            lastName: user.lastName,
                            troopId: user.troopId,
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

        createTroopsViaApi:
            async (troops) => {
                for (const troop of troops) {
                    await axios({
                        url: Uris.adminCreateTroop,
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        data: {
                            id: troop.id,
                            name: troop.name
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