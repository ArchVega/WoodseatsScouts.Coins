import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {MemberCompleteDto, MemberDto, MemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";

export default function MemberApiService() {
  return {
    async fetchMember(memberCode: string): Promise<AxiosResponse<MemberDto>> {
      return await apiClient.get(Uris.scouts().members().memberByCode(memberCode));
    },

    async fetchMemberComplete(memberCode: string): Promise<AxiosResponse<MemberCompleteDto>> {
      return await apiClient.get(Uris.scouts().members().fetchMemberComplete(memberCode));
    },

    async fetchMembers(): Promise<AxiosResponse<MemberPointsSummaryDto[]>> {
      return await apiClient.get(Uris.scouts().members().membersWithPointSummary());
    },

    async photo(photoImagePath: string): Promise<AxiosResponse<string>> {
      return await apiClient.get(Uris.scouts().members().memberPhoto(photoImagePath));
    }
  }
}