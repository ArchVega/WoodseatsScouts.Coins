import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {CoinDto, ScoutMemberDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";

export default function CoinApiService() {
  return {
    async fetchCoin(coinCode: string, memberCode: string): Promise<AxiosResponse<CoinDto>> {
      return await apiClient.put(Uris.coins().assign(coinCode, memberCode));
    },

    async addPointsToMember(member: ScoutMemberDto, coins: CoinDto[]) {
      const payload = {
        coinCodes: coins.map(x => x.code)
      }

      return await apiClient.put(Uris.scouts().members().addPointsToMember(member.id), payload)
    }
  }
}