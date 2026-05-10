import Uris from "./Uris.ts";
import {apiClient} from "./apiClient.ts";

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