import axios, {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {MemberDto, MemberPointsSummaryDto} from "../../types/ServerTypes.ts";

function MemberApiService() {
  return {
    async fetchMember(memberCode: string): Promise<AxiosResponse<MemberDto>> {
      return await axios.get(Uris.memberByCode(memberCode));
    },

    async fetchMemberWithPoints(memberQrCode) {
      const uri = Uris.memberWithPoints(memberQrCode);
      return await axios.get(uri);
    },

    async fetchMembers(): Promise<AxiosResponse<MemberPointsSummaryDto[]>> {
      const uri = Uris.membersWithPointSummary()
      return await axios.get(uri);
    }
  }
}

export default MemberApiService