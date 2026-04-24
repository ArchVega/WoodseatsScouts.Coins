import getAppSettings from "../../AppSettings.ts";
import axios from "axios";
import {logDebug, logError} from "../../components/logging/Logger.ts";
import Uris from "./Uris.ts";

const baseUri: string = getAppSettings().VITE_WEB_API_URI + "/api"

export const apiClient = axios.create({
  baseURL: baseUri
})

apiClient.interceptors.request.use((config) => {
  logDebug("Request started");
  return config;
});

apiClient.interceptors.response.use(
  (response) => {
    logDebug("Response received");

    const data = response.data;

    if (data?.computedImagePath) {
      data.clientComputedImageUri = response.config.baseURL + "/" + data.computedImagePath;
    } else if (Array.isArray(data)) {
      data.forEach((item) => {
        if (item.computedImagePath) {
          item.clientComputedImageUri = response.config.baseURL + "/" + item.computedImagePath;
        }
      })
    }

    return response;
  },
  (error) => {
    logError("Request failed");
    return Promise.reject(error);
  }
);