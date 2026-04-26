import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {ScoutMemberCompleteDto, ScoutMemberDto, ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";
import type {UpdateScoutMemberRequestPayload} from "../../types/ClientTypes.ts";

export default function MemberApiService() {
  return {
    async getMember(memberCode: string): Promise<AxiosResponse<ScoutMemberDto>> {
      return await apiClient.get(Uris.scouts().members().memberByCode(memberCode));
    },

    async getMemberComplete(scoutMemberId: number): Promise<AxiosResponse<ScoutMemberCompleteDto>> {
      return await apiClient.get(Uris.scouts().members().getMemberComplete(scoutMemberId));
    },

    async getMembers(): Promise<AxiosResponse<ScoutMemberPointsSummaryDto[]>> {
      return await apiClient.get(Uris.scouts().members().membersWithPointSummary());
    },

    async photo(photoImagePath: string): Promise<AxiosResponse<string>> { // todo: rename to getPhoto, check usage
      return await apiClient.get(Uris.scouts().members().memberPhoto(photoImagePath));
    },

    async updateScoutMember(scoutMemberId: number, payload: UpdateScoutMemberRequestPayload): Promise<ScoutMemberCompleteDto> {
      return await apiClient.put(Uris.scouts().members().memberById(scoutMemberId), payload).then(x => x.data)
    }
  }
}