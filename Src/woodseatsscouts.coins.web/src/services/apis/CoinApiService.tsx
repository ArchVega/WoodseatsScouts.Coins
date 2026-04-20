import axios, {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {CoinDto} from "../../types/ServerTypes.ts";
// import {logApi} from "../components/logging/Logger";

function CoinApiService() {
    return {
        async fetchCoin(coinCode: string, memberCode: string): Promise<AxiosResponse<CoinDto>> {
            const uri = Uris.pointValueFromCode(coinCode, memberCode)

            return await axios.get(uri);
        },

        async addPointsToMember(member, coins) {
            const payload = {
                coinCodes: coins.map(x => x.code)
            }

            return await axios.put(Uris.addPointsToMember(member.memberId), payload)
                .catch(reason => {
                    console.error(reason)
                })
        }
    }
}

export default CoinApiService