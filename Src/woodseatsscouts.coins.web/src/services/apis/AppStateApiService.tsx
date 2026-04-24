import axios from "axios";
import Uris from "./Uris.ts";

export default function AppStateApiService() {
  return {
    getAppSate: (responseFunc) => {
      async function fetch() {
        const response = await axios.get(Uris.appState);
        return response.data
      }

      fetch().then(response => {
        responseFunc(response)
      });
    },

    getAppVersion: (responseFunc) => {
      async function fetch() {
        const response = await axios.get(Uris.appVersion);
        return response.data
      }

      fetch().then(response => {
        responseFunc(response)
      });
    },

    getScoutGroups: async () => {
      return await axios.get(Uris.scoutGroups());

    },

    getSections: async () => {
      return await axios.get(Uris.sections());
    },
  }
}