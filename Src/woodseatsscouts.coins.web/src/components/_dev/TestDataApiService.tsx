import Uris from "../../services/apis/Uris.ts";
import {apiClient} from "../../services/apis/apiClient.ts";

export default function TestDataApiService() {
    return {
        async getUnscavengedCoins() {
            const uri = Uris. testDataCoins
            const response = await apiClient.get(uri);

            return response.data.filter(x => !x.isAlreadyScavenged)
        },

        async getMembers() {
            const uri = Uris.scouts().members().membersWithPointSummary() // todo: this is overkill but was available - make a 'basic' version
            const response = await apiClient.get(uri);

            return response.data
        }
    }
}