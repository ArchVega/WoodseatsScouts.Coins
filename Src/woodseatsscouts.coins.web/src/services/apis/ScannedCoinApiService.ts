import {type AxiosResponse} from "axios";
import Uris from "./Uris.ts";
import type {ScoutMemberCompleteDto, ScoutMemberDto, ScoutMemberPointsSummaryDto} from "../../types/ServerTypes.ts";
import {apiClient} from "./apiClient.ts";
import type {UpdateScoutMemberRequestPayload} from "../../types/ClientTypes.ts";

export default function ScannedCoinApiService() {
  return {
    async updateScannedCoinPoints(scannedCoinId: number, newPointsValue: number): Promise<void> {
      return await apiClient.put(Uris.scans().coins().resourceEntityPath(scannedCoinId), {
        newPointsValue: newPointsValue
      });
    },
    async deleteScannedCoin(scannedCoinId: number) {
      return await apiClient.delete(Uris.scans().coins().resourceEntityPath(scannedCoinId));
    }
  }
}