import axios, {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {MemberCompleteDto, MemberDto, MemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {logObject} from "../../components/logging/Logger.ts";

export default function MemberApiService() {
  return {
    async fetchMember(memberCode: string): Promise<AxiosResponse<MemberDto>> {
      return await axios.get(Uris.scouts().members().memberByCode(memberCode));
    },

    async fetchMemberComplete(memberCode: string): Promise<AxiosResponse<MemberCompleteDto>> {
      const uri = Uris.scouts().members().fetchMemberComplete(memberCode);
      return await axios.get(uri);
    },

    async fetchMembers(): Promise<AxiosResponse<MemberPointsSummaryDto[]>> {
      const uri = Uris.scouts().members().membersWithPointSummary()
      return await axios.get(uri);
    },

    async photo(photoImagePath: string): Promise<AxiosResponse<string>> {
      return await axios.get(Uris.scouts().members().memberPhoto(photoImagePath));
    }
  }
}