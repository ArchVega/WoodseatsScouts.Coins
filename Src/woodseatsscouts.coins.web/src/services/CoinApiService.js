import axios from "axios";
import {logApi} from "../components/logging/Logger";

function CoinApiService() {
    return {
        async fetchCoin(coinQrCode, memberQrCode) {
            const uri = `http://localhost:7167/home/GetPointValueFromCode?code=${coinQrCode}&memberCode=${memberQrCode}`
            logApi(uri)
            return await axios.get(uri);
        },

        async addPointsToMember(member, coins) {
            const payload = {
                memberId: member.memberId,
                coinCodes: coins.map(x => x.code)
            }

            return await axios.post('http://localhost:7167/home/AddPointsToMember', payload)
                .catch(reason => {
                    console.error(reason)
                })
        }
    }
}

export default CoinApiService