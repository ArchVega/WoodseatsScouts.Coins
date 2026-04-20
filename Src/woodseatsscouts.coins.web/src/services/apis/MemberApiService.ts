import axios, {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {MemberDto} from "../../types/ServerTypes.ts";

function MemberApiService() {
  return {
    async fetchMember(memberCode: string): Promise<AxiosResponse<MemberDto>> {
      return await axios.get(Uris.memberByCode(memberCode));
    },

    async fetchMemberWithPoints(memberQrCode) {
      const uri = Uris.memberWithPoints(memberQrCode);
      return await axios.get(uri);
    },

    async fetchMembers() {
      const uri = Uris.member
      return await axios.get(uri);
    }
  }
}

export default MemberApiService