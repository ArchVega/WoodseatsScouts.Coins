import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {ScoutMemberCompleteDto, ScoutMemberDto, ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";
import type {UpdateScoutMemberRequestPayload} from "../../types/ClientTypes.ts";

export default function ScanSessionApiService() {
  return {
    async deleteScanSession(scannedSessionId: number): Promise<void> {
      return await apiClient.delete(Uris.scans().sessions().resourceEntityPath(scannedSessionId));
    }
  }
}