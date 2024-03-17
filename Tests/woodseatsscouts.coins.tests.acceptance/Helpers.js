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

        setDeadlineTime: async (daysToAdd) => {
            await axios({
                url: Uris.sutScavengerHuntDeadline(daysToAdd),
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

            membersResponse.data.forEach(member => {
                const source = member.lastName + "-" + member.firstName + ".png";
                const target = member.id + ".jpg";

                fs.cp(
                    `./testImages/${source}`,
                    `./../../Src/woodseatsscouts.coins.web/public/member-images/${target}`,
                    (err) => {
                    });
            })

            await axios({
                url: Uris.sutSetMemberPropertyHasImageToTrue,
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: {}
            }).then(response => {
                console.log(response)
            }).catch(reason => {
                console.error(reason)
            })
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

        createTroopsViaApi: async (troops) => {
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