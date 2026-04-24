import axios, {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {MemberCompleteDto, MemberDto, MemberPointsSummaryDto} from "../../types/ServerTypes.ts";

function MemberApiService() {
  return {
    async fetchMember(memberCode: string): Promise<AxiosResponse<MemberDto>> {
      return await axios.get(Uris.memberByCode(memberCode));
    },

    // async fetchMemberWithPoints(memberCode: string) {
    //   const uri = Uris.memberWithPoints(memberCode);
    //   return await axios.get(uri);
    // },

    async fetchMemberComplete(memberCode: string): Promise<AxiosResponse<MemberCompleteDto>> {
      const uri = Uris.fetchMemberComplete(memberCode);
      return await axios.get(uri);
    },

    async fetchMembers(): Promise<AxiosResponse<MemberPointsSummaryDto[]>> {
      const uri = Uris.membersWithPointSummary()
      return await axios.get(uri);
    }
  }
}

export default MemberApiService