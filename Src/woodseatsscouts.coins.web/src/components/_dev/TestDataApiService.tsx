import axios from "axios";
import Uris from "../../services/apis/Uris.ts";

export default function TestDataApiService() {
    return {
        async getUnscavengedCoins() {
            const uri = Uris.testDataCoins
            const response = await axios.get(uri);

            return response.data.filter(x => !x.isAlreadyScavenged)
        },

        async getMembers() {
            const uri = Uris.members
            const response = await axios.get(uri);

            return response.data
        }
    }
}