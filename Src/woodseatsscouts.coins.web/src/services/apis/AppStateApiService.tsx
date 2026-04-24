import axios from "axios";
import Uris from "./Uris.ts";

export default function AppStateApiService() {
  return {
    getAppSate: (responseFunc: (response: any) => void) => {
      async function fetchAppState() {
        const response = await axios.get(Uris.application().mode());
        return response.data
      }

      fetchAppState().then(response => {
        responseFunc(response)
      });
    },

    getAppVersion: (responseFunc) => {
      async function fetch() {
        const response = await axios.get(Uris.application().appVersion());
        return response.data
      }

      fetch().then(response => {
        responseFunc(response)
      });
    },

    getScoutGroups: async () => {
      return await axios.get(Uris.scouts().groups().resourcePath);

    },

    getSections: async () => {
      return await axios.get(Uris.scouts().sections().resourcePath);
    },
  }
}