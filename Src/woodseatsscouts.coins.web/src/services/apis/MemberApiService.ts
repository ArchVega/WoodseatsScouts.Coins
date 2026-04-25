import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {ScoutMemberCompleteDto, ScoutMemberDto, ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";
import type {UpdateScoutMemberRequestPayload} from "../../types/ClientTypes.ts";

export default function MemberApiService() {
  return {
    async fetchMember(memberCode: string): Promise<AxiosResponse<ScoutMemberDto>> {
      return await apiClient.get(Uris.scouts().members().memberByCode(memberCode));
    },

    async fetchMemberComplete(memberCode: string): Promise<AxiosResponse<ScoutMemberCompleteDto>> {
      return await apiClient.get(Uris.scouts().members().fetchMemberComplete(memberCode));
    },

    async fetchMembers(): Promise<AxiosResponse<ScoutMemberPointsSummaryDto[]>> {
      return await apiClient.get(Uris.scouts().members().membersWithPointSummary());
    },

    async photo(photoImagePath: string): Promise<AxiosResponse<string>> {
      return await apiClient.get(Uris.scouts().members().memberPhoto(photoImagePath));
    },

    async updateScoutMember(scoutMemberId: number, payload: UpdateScoutMemberRequestPayload): Promise<ScoutMemberDto> {
      return await apiClient.put(Uris.scouts().members().memberById(scoutMemberId), payload).then(x => x.data)
    }
  }
}