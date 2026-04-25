import Uris from "../../services/apis/Uris.ts";
import {apiClient} from "../../services/apis/apiClient.ts";
import type {ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import type {UnscavengedCoin} from "../../types/ClientTypes.ts";

export default function TestDataApiService() {
    return {
        async getUnscavengedCoins(): Promise<UnscavengedCoin[]> {
            const uri = Uris.system().testDataCoins()
            const response = await apiClient.get(uri);

            return response.data.filter(x => !x.isAlreadyScavenged)
        },

        async getMembers(): Promise<ScoutMemberPointsSummaryDto[]> {
            const uri = Uris.scouts().members().membersWithPointSummary() // todo: this is overkill but was available - make a 'basic' version
            const response = await apiClient.get(uri);

            return response.data
        }
    }
}