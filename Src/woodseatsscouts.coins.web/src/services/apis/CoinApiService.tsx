import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {CoinDto, MemberDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";

export default function CoinApiService() {
  return {
    async fetchCoin(coinCode: string, memberCode: string): Promise<AxiosResponse<CoinDto>> {
      const uri = Uris.coins().scans(coinCode, memberCode)

      return await apiClient.get(uri);
    },

    async addPointsToMember(member: MemberDto, coins: CoinDto[]) {
      const payload = {
        coinCodes: coins.map(x => x.code)
      }

      return await apiClient
        .put(Uris.scouts().members().addPointsToMember(member.id), payload).catch(reason => {
          console.error(reason)
        })
    }
  }
}