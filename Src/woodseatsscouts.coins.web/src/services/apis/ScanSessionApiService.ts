import Uris from "./Uris.ts";
import {apiClient} from "./apiClient.ts";

export default function ScanSessionApiService() {
  return {
    async deleteScanSession(scannedSessionId: number): Promise<void> {
      return await apiClient.delete(Uris.scans().sessions().resourceEntityPath(scannedSessionId));
    }
  }
}