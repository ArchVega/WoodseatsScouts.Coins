// Gets all the coins from the DB.
import axios from "axios";
import Uris from "../pageModels/Uris";

const ScavengerHunt = async () => {
    const coinsResponse = await axios.get(Uris.coinsGet)
    const _coins = coinsResponse.data.map(x => {
        return {
            code: x.code,
            value: x.value,
            fullName: x.fullName,
            isAlreadyScavenged: x.isAlreadyScavenged
        }
    });

    return {
        getUnscavengedCoinByValue: async (userFullName, values) => {
            const coins = [];

            for (const value of values) {
                const unscavengedCoin = _coins.filter(x => !x.isAlreadyScavenged && x.value === value)[0]
                unscavengedCoin.isAlreadyScavenged = true;
                unscavengedCoin.fullName = userFullName;
                coins.push(unscavengedCoin);
            }

            return coins;
        },

        allocateTestCoinToUser: (specificCoinToBeShared, userFullName) => {
            specificCoinToBeShared.isAlreadyScavenged = true;
            specificCoinToBeShared.fullName = userFullName;
        },

        /* Does not assign a member to an unscavenged coin.*/
        peekUnscavengedCoin: async (value) => {
            return _coins.filter(x => !x.isAlreadyScavenged && x.value === value)[0]
        },

        getAnAlreadyScavengedCoin: async (userFullName) => {
            return _coins.filter(x => x.isAlreadyScavenged && x.fullName !== userFullName)[0]
        }
    }
}

export default ScavengerHunt
