import axios from "axios";
import {logApi} from "../components/logging/Logger";
import Uris, {addPointsToMemberUri, BaseUri, getPointValueFromCodeUri} from "./Uris";

function CoinApiService() {
    return {
        async fetchCoin(coinQrCode, memberQrCode) {
            const uri = Uris.pointValueFromCode(coinQrCode, memberQrCode)
            logApi(uri)
            return await axios.get(uri);
        },

        async addPointsToMember(member, coins) {
            const payload = {
                memberId: member.memberId,
                coinCodes: coins.map(x => x.code)
            }

            return await axios.post(Uris.addPointsToMember, payload)
                .catch(reason => {
                    console.error(reason)
                })
        }
    }
}

export default CoinApiService