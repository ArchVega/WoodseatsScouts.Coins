import axios, {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {CoinDto, MemberDto} from "../../types/ServerTypes.ts";

export default function CoinApiService() {
  return {
    async fetchCoin(coinCode: string, memberCode: string): Promise<AxiosResponse<CoinDto>> {
      const uri = Uris.pointValueFromCode(coinCode, memberCode)

      return await axios.get(uri);
    },

    async addPointsToMember(member: MemberDto, coins: CoinDto[]) {
      const payload = {
        coinCodes: coins.map(x => x.code)
      }

      return await axios
        .put(Uris.addPointsToMember(member.id), payload).catch(reason => {
          console.error(reason)
        })
    }
  }
}